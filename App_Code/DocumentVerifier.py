import os
import re
import numpy as np
import tensorflow as tf
from tensorflow.keras.models import load_model
from PIL import Image
import PyPDF2
import pdf2image
import cv2
import pytesseract
from pytesseract import Output

# Configure paths and constants
MODEL_SAVE_PATH = r"C:\Users\RGMWB\PycharmProjects\FileClassification\document_model\document_classifier.keras"

INPUT_SIZE = (224, 224)
WHITE_THRESHOLD = 0.95
MIN_TEXT_THRESHOLD = 10
MIN_CONFIDENCE = 0.7

# Document class labels
CLASS_LABELS = {
    0: 'PAN Card',
    1: 'Aadhaar Card',
    2: 'Driving License',
    3: 'Birth Certificate',
    4: 'Voter ID',
    5: 'Caste Certificate'
}

# Regex patterns for document numbers
AADHAAR_REGEX = re.compile(r"\b\d{4}\s?\d{4}\s?\d{4}\b")
PAN_REGEX = re.compile(r"[A-Z]{5}\d{4}[A-Z]")
DL_REGEX = re.compile(r"[A-Z]{2}\d{2,4}\s?\d{4}\s?\d{4,7}")
EPIC_REGEX = re.compile(r"^[A-Z]{3}[0-9]{7}$")

class DocumentVerifier:
    def __init__(self, model_path=None):
        self.model = load_model(model_path) if model_path and os.path.exists(model_path) else None
        self.input_size = INPUT_SIZE
        
    def extract_text_with_ocr(self, image, languages='eng+hin+ben'):
        """Extract text from image using OCR with multilingual support"""
        try:
            # Convert to grayscale and apply thresholding
            gray = cv2.cvtColor(np.array(image), cv2.COLOR_RGB2GRAY)
            _, threshold = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)

            # Apply additional preprocessing
            denoised = cv2.fastNlMeansDenoising(threshold, h=10)
            custom_config = f'--oem 3 --psm 6 -l {languages}'
            text = pytesseract.image_to_string(denoised, config=custom_config)
            return text.strip()
        except Exception as e:
            print(f"OCR Error: {str(e)}")
            return ""

    def extract_pdf_text(self, pdf_path):
        """Extract text from PDF with multilingual OCR support"""
        text = ""
        try:
            # First try direct text extraction
            with open(pdf_path, 'rb') as file:
                reader = PyPDF2.PdfReader(file)
                for page in reader.pages:
                    text += page.extract_text() or ""

            # If direct extraction didn't get enough text, try OCR
            if len(text.strip()) < MIN_TEXT_THRESHOLD:
                images = pdf2image.convert_from_path(pdf_path)
                for img in images:
                    text += "\n" + self.extract_text_with_ocr(img)
        except Exception as e:
            print(f"PDF Extraction Error: {str(e)}")
            return ""
        return text.strip()

    def detect_document_type(self, text):
        """Detect document type based on extracted text"""
        text_lower = text.lower()

        # Check by document number patterns first (most reliable)
        if AADHAAR_REGEX.search(text):
            return CLASS_LABELS[1], 1
        if PAN_REGEX.search(text):
            return CLASS_LABELS[0], 0
        if DL_REGEX.search(text):
            return CLASS_LABELS[2], 2
        if EPIC_REGEX.search(text):
            return CLASS_LABELS[4], 4

        # Check for keywords
        if "aadhaar" in text_lower or "uid" in text_lower:
            return CLASS_LABELS[1], 1
        if "pan" in text_lower or "permanent account" in text_lower:
            return CLASS_LABELS[0], 0
        if "driving" in text_lower or "license" in text_lower or "dl no" in text_lower:
            return CLASS_LABELS[2], 2
        if "voter" in text_lower or "election" in text_lower or "epic" in text_lower:
            return CLASS_LABELS[4], 4
        if "birth" in text_lower or "certificate" in text_lower:
            return CLASS_LABELS[3], 3
        if "caste" in text_lower or "category" in text_lower:
            return CLASS_LABELS[5], 5

        return "UNKNOWN", -1

    def is_white_page(self, image):
        """Check if image is predominantly white/blank"""
        gray = cv2.cvtColor(np.array(image), cv2.COLOR_RGB2GRAY)
        _, threshold = cv2.threshold(gray, 240, 255, cv2.THRESH_BINARY)
        white_ratio = np.sum(threshold == 255) / threshold.size
        return white_ratio > WHITE_THRESHOLD

    def preprocess_image(self, img):
        """Convert image to model input format"""
        img = img.resize(self.input_size)
        img_array = tf.keras.preprocessing.image.img_to_array(img)
        img_array = np.expand_dims(img_array, axis=0) / 255.0
        return img_array

    def verify_document(self, file_path, expected_doc_type=None, expected_id=None):
        """Verify a document file"""
        result = {
            "valid": False,
            "document_type": None,
            "detected_id": None,
            "confidence": 0.0,
            "matches_expected": False,
            "errors": []
        }

        if not os.path.exists(file_path):
            result["errors"].append("File not found")
            return result

        file_ext = os.path.splitext(file_path)[1].lower()

        try:
            if file_ext == '.pdf':
                extracted_text = self.extract_pdf_text(file_path)
                images = pdf2image.convert_from_path(file_path)
                img = images[0] if images else None
            elif file_ext in ('.jpg', '.jpeg', '.png'):
                img = Image.open(file_path)
                extracted_text = self.extract_text_with_ocr(img)
            else:
                result["errors"].append("Unsupported file format")
                return result

            # Check for blank page
            if img and self.is_white_page(img):
                result["errors"].append("Document appears to be blank")

            # Text-based detection
            if extracted_text:
                detected_type, class_idx = self.detect_document_type(extracted_text)
                
                # Try to find the ID number in the text
                detected_id = None
                if detected_type == "PAN Card":
                    pan_match = PAN_REGEX.search(extracted_text)
                    detected_id = pan_match.group(0) if pan_match else None
                elif detected_type == "Aadhaar Card":
                    aadhaar_match = AADHAAR_REGEX.search(extracted_text)
                    detected_id = aadhaar_match.group(0) if aadhaar_match else None
                
                result.update({
                    "document_type": detected_type,
                    "detected_id": detected_id,
                    "confidence": 0.9 if detected_type != "UNKNOWN" else 0.0
                })

                # If we have an expected ID, check if it matches
                if expected_id and detected_id:
                    # Clean both IDs for comparison
                    clean_expected = re.sub(r'\s+', '', expected_id).upper()
                    clean_detected = re.sub(r'\s+', '', detected_id).upper()
                    result["matches_expected"] = clean_expected in clean_detected or clean_detected in clean_expected
                    
                    # For PAN cards, do exact matching
                    if detected_type == "PAN Card":
                        result["matches_expected"] = clean_expected == clean_detected

            # If text detection failed, fall back to model prediction
            if result["document_type"] == "UNKNOWN" and self.model and img:
                processed_img = self.preprocess_image(img)
                predictions = self.model.predict(processed_img)
                predicted_class = np.argmax(predictions[0])
                confidence = float(predictions[0][predicted_class])
                model_type = CLASS_LABELS.get(predicted_class, "UNKNOWN")

                result.update({
                    "document_type": model_type,
                    "confidence": confidence
                })

            # Final validation decision
            if result["document_type"] != "UNKNOWN" and result["confidence"] >= MIN_CONFIDENCE:
                if expected_doc_type:
                    # Compare detected type with expected type
                    result["valid"] = (result["document_type"].lower() == expected_doc_type.lower())
                else:
                    result["valid"] = True
                
                # If we have an expected ID but couldn't detect one, mark as invalid
                if expected_id and not result["detected_id"]:
                    result["valid"] = False
                    result["errors"].append("Could not verify ID number in document")

        except Exception as e:
            result["errors"].append(f"Processing error: {str(e)}")

        return result

# Main function to be called from VB.NET
def verify_document(file_path, doc_type=None, doc_id=None):
    """
    Main function to verify a document
    :param file_path: Path to the document file
    :param doc_type: Expected document type (optional)
    :param doc_id: Expected document ID (optional)
    :return: Dictionary with verification results
    """
    verifier = DocumentVerifier(MODEL_SAVE_PATH)
    return verifier.verify_document(file_path, doc_type, doc_id)

# Example usage
if __name__ == "__main__":
    import sys
    import json
    try:
        file_path = sys.argv[1]
        expected_doc_type = sys.argv[2] if len(sys.argv) > 2 else None
        expected_id = sys.argv[3] if len(sys.argv) > 3 else None

        verifier = DocumentVerifier(MODEL_SAVE_PATH)
        result = verifier.verify_document(file_path, expected_doc_type, expected_id)
        print(json.dumps(result))

    except Exception as e:
        print(json.dumps({
            "valid": False,
            "document_type": None,
            "detected_id": None,
            "confidence": 0.0,
            "matches_expected": False,
            "errors": [str(e)]
        }))