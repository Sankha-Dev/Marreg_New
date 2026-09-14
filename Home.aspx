<%@ Page Title="Home" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link rel="stylesheet" href="css/owl-carousel.css"/>
    <script src="js/owl.carousel.min.js"></script>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        var readMoreButton = document.getElementById("aboutReadMore");
        var details = document.getElementById("about-details");

        if (!readMoreButton || !details) {
            return;
        }

        readMoreButton.addEventListener("click", function () {
            var isExpanded = details.style.display === "block";
            details.style.display = isExpanded ? "none" : "block";
            readMoreButton.textContent = isExpanded ? "Know More" : "Show Less";
            readMoreButton.setAttribute("aria-expanded", String(!isExpanded));
        });
    });
    $(document).ready(function () {

        var owl = $('.owl-carousel');

        owl.owlCarousel({
            lazyLoad: true,
            loop: true,
            autoWidth: false,
            autoplay: true,
            autoplayTimeout: 2000,
            autoplayHoverPause: true,
            nav: true,
            dots: true,
            margin: 0,

            navText: [
                "<i class='fa fa-arrow-left' aria-hidden='true'></i>",
                "<i class='fa fa-arrow-right' aria-hidden='true'></i>"
            ],

            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                769: {
                    items: 1
                }
            }
        });
    });
</script>
     <!-- banner -->
        <section class="accessibility banner">
            <div class="container">
                <!-- slider -->
                <div class="owl-carousel owl-theme owl-loaded owl-drag">
                    <div class="item    rounded mb-0 d-flex align-items-center justify-content-between">

                        <div class="row accessibility-banner position-relative">
                            <div class="col-sm-6 col-12">
                                <div class="accessibility-banner-content">
                                    <div class="text-center d-block d-md-none">
                                        <img src="images/slideshow/Pic1.jpg" alt="Responsive" loading="lazy" style="height: 200px;"/>
                                    </div>
                                    <h1><span>The Hindu Marriage Act, 1955</span></h1>
                                    <p>
                                        <strong>
                                            Application Can Be Submitted To Marriage Officer. After Social Marriage. Objection Period Is 7 Days From The Date Of Received Application. Marriage Should Be Registered Within 6 Calendar Months From The Date
                                            Of Receipt Of Application.
                                        </strong>
                                    </p>
                                </div>
                            </div>
                            <div class="col-sm-6 col-12 text-end d-none d-sm-block">
                                <div class="text-end">
                                    <img src="images/slideshow/Pic1.jpg" alt="Responsive" loading="lazy" style="height: 350px;"/>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="item  rounded d-flex align-items-center justify-content-between">

                        <div class="row accessibility-banner position-relative">
                            <div class="col-sm-6 col-12">
                                <div class="accessibility-banner-content">
                                    <div class="text-center d-block d-md-none">
                                        <img src="images/slideshow/Pic2.jpg" alt="Responsive" loading="lazy" style="height: 200px;"/>
                                    </div>
                                    <h1><span style="color: #30b5b2;"> The Special Marriage Act, 1954 (Under Section 13)</span></h1>
                                    <p> <strong>Under This Act Of Marriage,Declaration/Solemnisation Process Should Start After 30 Days From The Date Of Receipt Of Notice Date And End Within 3 Months Of This Notice Period.</strong></p>
                                </div>
                            </div>
                            <div class="col-sm-6 col-12 d-none d-sm-block">
                                <div class="text-end">
                                    <img src="images/slideshow/Pic2.jpg" alt="Responsive" loading="lazy" style="height: 350px;"/>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="item  rounded d-flex align-items-center justify-content-between">

                        <div class="row accessibility-banner position-relative">
                            <div class="col-sm-6 col-12">
                                <div class="accessibility-banner-content">
                                    <div class="text-center d-block d-md-none">
                                        <img src="images/slideshow/Pic3.jpg" alt="Responsive" loading="lazy" style="height: 200px;">
                                    </div>
                                    <h1><span style="color: #075705;">The Special Marriage Act, 1954 (Under Section 16)</span></h1>
                                    <p><strong>Under This Act Of Marriage, Application Can Be Submitted To Marriage Officer At Least After 30days From The Date Of Marriage. Objection Period 30days From The Date Of Received Application. Marriage Should Be Registered Within 6 Calendar Months From The Date Of Received Application.</strong></p>
                                </div>
                            </div>
                            <div class="col-sm-6 col-12 d-none d-sm-block">
                                <div class="text-end">
                                    <img src="images/slideshow/Pic3.jpg" alt="Responsive" loading="lazy" style="height: 350px;"/>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="item  rounded d-flex align-items-center justify-content-between">

                        <div class="row accessibility-banner position-relative">
                            <div class="col-sm-6 col-12">
                                <div class="accessibility-banner-content">
                                    <div class="text-center d-block d-md-none">
                                        <img src="images/slideshow/Pic4.jpg" alt="Responsive" loading="lazy" style="height: 200px;"/>
                                    </div>
                                    <h1><span style="color: #075705;">The Indian Christian Marriage Act, 1872</span></h1>
                                    <p><strong>Under This Act Of Marriage, Both The Bride And The Groom Should Be More Than 21 Years Of Age. In Case Age Of Bride Is Below 21 Years But Above 18 Years In Such Cases, A Letter Of Consent Is Required From The Guardian/Parent.</strong></p>
                                </div>
                            </div>
                            <div class="col-sm-6 col-12 d-none d-sm-block">
                                <div class="text-end">
                                    <img src="images/slideshow/Pic4.jpg" alt="Responsive" loading="lazy" style="height: 350px;"/>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="item  rounded d-flex align-items-center justify-content-between">

                        <div class="row accessibility-banner position-relative">
                            <div class="col-sm-6 col-12">
                                <div class="accessibility-banner-content">
                                    <div class="text-center d-block d-md-none">
                                        <img src="images/slideshow/Pic5.jpg" alt="Responsive" loading="lazy" style="height: 200px;">
                                    </div>
                                    <h1><span style="color: #075705;">The Parsi Marriage & Divorce Act, 1936</span></h1>
                                    <p><strong>A Parsi Marriage Is Solemnised According To The Parsi Form Of Ceremony Called "Ashirvad" By A Priest In The Presence Of Two Parsi Witnesses Other Than Such Priest Or In The Case Of Any Parsi (Whether Such Parsi Has Changed His Or Her Religion Or Domicile Or Not).</strong></p>
                                </div>
                            </div>
                            <div class="col-sm-6 col-12 d-none d-sm-block">
                                <div class="text-end">
                                    <img src="images/slideshow/Pic5.jpg" alt="Responsive" loading="lazy" style="height: 350px;"/>
                                </div>
                            </div>

                        </div>

                    </div>
                </div>
                <!-- slider end -->


            </div>

        </section>

        <!-- latest-news -->
        <section class="latest-news">
            <div class="container">
                <div class="latest-news-content d-flex align-items-center justify-content-between">
                    <h5 class="d-flex gap-2"><img src="images/star.svg" alt="star"/> LATEST</h5>
                    <marquee behavior="" direction="">
                        <span style="font-size:15px; vertical-align:central; color:red; margin-bottom:5px; text-transform:uppercase">
                            << ALL MARRIAGE REGISTRARS who have been allowed by RGM to Register Marriages online should immediately start using the Portal >> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  << <span style="color:blue"> <b> Important Document </b> </span> >> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:LinkButton ID="btnPDF"  Font-Underline="false" CommandArgument="MARREG_Advt.pdf" runat="server" CssClass="lbl"> <span class="blink-image"> Click Here to Download &nbsp;&nbsp;&nbsp;<img style="width:25px; vertical-align:middle;" src="../images/icons/download_logo.svg"> </span></asp:LinkButton>
                        </span>
                    </marquee>
                </div>
            </div>
        </section>

        <!-- counter -->
        <section class="counter">
            <div class="container">
                <div class="row">
                    <div class="col-12 col-sm-3">
                        <div class="counter_box text-center">
                            <!--<img src="images/23x32.png" alt="icon">-->
                            <h1>00000</h1>
                            <p><b>Certificates Generated from 1st January of Current Year</b></p>
                        </div>
                    </div>
                    <div class="col-12 col-sm-3">
                        <div class="counter_box text-center">
                            <!--<img src="images/23x32.png" alt="icon">-->
                            <h1>00000</h1>
                            <p> <b> Certificates Generated from 1st of Current Month</b> </p>
                        </div>
                    </div>
                    <div class="col-12 col-sm-3">
                        <div class="counter_box text-center">
                            <!--<img src="images/23x32.png" alt="icon">-->
                            <h1>00000</h1>
                            <p><b>Online MO</b></p>
                        </div>
                    </div>
                    <div class="col-12 col-sm-3">
                        <div class="counter_box text-center">
                            <!--<img src="images/23x32.png" alt="icon">-->
                            <h1>00000</h1>
                            <p><b>Visitors Count</b></p>
                        </div>
                    </div>

                </div>
                <div class="transparent-line text-center">
                    <img src="images/line.svg" alt="line">
                </div>
            </div>
        </section>

        <!-- about section -->
        <section class="about">
            <div class="container">
                <div class="row">

                    <div class="col-sm-5">
                        <div class="about-content" id="about_txt_eng">

                            <h2>About Department</h2>

                            <div id="about-short">
                                <p>We register marriages.</p>
                                <p>We add value to marriages.</p>
                                <p>We devise innovative service delivery model for registration of marriages to benefit the citizen.</p>
                                <p>As an application towards providing speedy services to the citizens, "<b>MARREG</b>" shall be totally web accessed.</p>
                            </div>

                            <div id="about-details">

                                <p>
                                    This shall enable greater mobility of registrars as the site may be
                                    accessed from any location using Internet facilities from any ISP.
                                </p>

                                <p>
                                    Overall responsibility to ensure that the policies and guidelines for
                                    "<b>MARREG</b>" are properly implemented shall rest upon the
                                    <b>Government of West Bengal</b> with
                                    <b>O/o Registrar General of Marriages</b> exercising oversight function.
                                </p>

                                <p>
                                    Computerisation of the process of registration of marriages necessitates
                                    the need to preserve data in electronic form and aid that the incumbents
                                    get a fair trial in the event of any dispute arising out of the marriage, viz.
                                </p>

                                <ul style="font-size: 14px">
                                    <li>Polygamy / Polyandry</li>
                                    <li>Legal separation</li>
                                    <li>Grant of citizenship</li>
                                    <li>Other legal and social angles</li>
                                </ul>

                            </div>

                             <button type="button" class="btn btn-primary btn-whit-outline" id="aboutReadMore" aria-controls="about-details" aria-expanded="false">Know More</button>

                        </div>
                    </div>

                    <div class="col-sm-1"></div>

                    <div class="col-sm-6">
                        <div class="position-relative about-c-img">
                            <img src="images/rgm_wb.png" loading="lazy">
                            <%--    <img src="images/so_rgm_wb_218x218.png"
                                     loading="lazy"
                                     class="about-img2">

                                <img src="images/ha_rgm_wb_206x206.png"
                                     loading="lazy"
                                     class="about-img3">

                                <img src="images/uda_rg_wb_101x101.png"
                                     loading="lazy"
                                     class="about-img4">--%>
                        </div>
                    </div>

                </div>
            </div>
        </section>

        <!-- Our Ministers -->
        <section class="our-ministers">
            <div class="container">
                <div class="our-ministers-heading">
                    <h2>Our Ministers</h2>
                </div>
                <div class="our-ministers-card">
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="card-modi-ji">
                                <img src="images/cm_wb_282x282.png" alt="modi ji">
                                <p>Hon’ble Chief Minister</p>
                                <h3>Shri Suvendu Adhikari</h3>
                                <div class="pmg-social d-flex gap-2 align-items-center justify-content-center mt-20">
                                    <a href=""><img src="images/linkedin.svg" alt="linkedin"></a>
                                    <a href=""><img src="images/facebook2.svg" alt="facebook"></a>
                                    <a href=""><img src="images/instagram.svg" alt="instagram"></a>
                                    <a href=""><img src="images/twitter2.svg" alt="linkedin"></a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-7">
                            <div class="pmg-team">

                                <div class="pmg-team-content d-flex gap-3 align-items-center">
                                    <img src="images/mos_wb_218x218.png" alt="name">
                                    <div class="pmg-team-title ashwini-ji">
                                        <p>Hon'ble Minister of State, Law Department</p>
                                        <h3>Shri Biraj Biswas</h3>
                                    </div>
                                </div>

                                <div class="pmg-team-content d-flex flex-column align-items-center">
                                    <img src="images/ps_wb_120x120.png" alt="name">
                                    <div class="pmg-team-title minister-ji text-center">
                                        <p>Principal Secretary, Law Department</p>
                                        <h3>Shri Subhradip Mitra</h3>
                                    </div>
                                </div>

                                <!--<div class="pmg-team-content d-flex flex-column align-items-center">
                                    <img src="images/ps_wb_120x120.png" alt="name">
                                    <div class="pmg-team-title minister-ji text-center">
                                        <p>Minister of State </p>
                                        <h3>Smt. Darshana Jardosh</h3>
                                    </div>
                                </div>-->

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>


        <!-- our service -->

        <section class="services-panel">
            <div class="container">
                <h2 class="section-heading mb-20">
                    Our Services
                </h2>

                <!-- tab services -->
                <div class="services-tab">
                    <ul class="nav nav-tabs border-bottom-0 d-flex align-items-center justify-content-center" style="margin-bottom:60px" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="nav-accordion-one-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-one" type="button" role="tab" aria-controls="nav-accordion-one" aria-selected="false" tabindex="-1"> Main Services</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="nav-accordion-two-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-two" type="button" role="tab" aria-controls="nav-accordion-two" aria-selected="true"> Additional Services</button>
                        </li>


                    </ul>
                </div>
                <!-- tab -->
                <div class="tab-content" id="nav-tabContent">
                    <div class="tab-pane fade faqPage active show" id="nav-accordion-one" role="tabpanel" aria-labelledby="nav-accordion-one-tab">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/apply_online.svg" alt="icon" loading="lazy">
                                    <h3> Apply Online</h3>
                                    <!--<p>Sub-Heading</p>-->
                                </div>
                            </div>
                            <!--<div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/objections.svg" alt="icon" loading="lazy">
                                    <h3> Objection</h3>-->
                                    <!--<p>Sub-Heading</p>-->
                                <!--</div>
                            </div>-->
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/transfers.svg" alt="icon" loading="lazy">
                                    <h3>Transfer</h3>
                                    <!--<p>Sub-Heading-2</p>-->
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/cc_badge.svg" alt="icon" loading="lazy">
                                    <h3>Certified Copy of Marriage Records</h3>
                                    <!--<p>Sub-Heading-3</p>-->
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/search.svg" alt="icon" loading="lazy">
                                    <h3>Search Marriage Records</h3>
                                    <!--<p>Sub-Heading-4</p>-->
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane fade faqPage" id="nav-accordion-two" role="tabpanel" aria-labelledby="nav-accordion-two-tab">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/objections.svg" alt="icon" loading="lazy">
                                    <h3>Objection</h3>
                                    <!--<p>Sub-Heading-4</p>-->
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/faq.svg" alt="icon" loading="lazy">
                                    <h3>FAQ</h3>
                                    <!--<p>Sub-Heading-4</p>-->
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/circulars.svg" alt="icon" loading="lazy">
                                    <h3>Circulars</h3>
                                    <!--<p>Sub-Heading-4</p>-->
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="service-card">
                                    <img src="images/icons/sitemap.svg" alt="icon" loading="lazy">
                                    <h3>Sitemap</h3>
                                    <!--<p>Sub-Heading-4</p>-->
                                </div>
                            </div>

                        </div>
                        </div>
                </div>
            </div>
        </section>


        <!-- Station Gallery -->

        <section class="station-gallery">
            <div class="container">
                <h2 class="section-heading mb-20">
                    Gallery
                </h2>

                <!-- tab services -->
                <div class="services-tab">
                    <ul class="state-tab-nav nav nav-tabs border-bottom-0 d-flex align-items-center gap-3" style="margin-bottom:60px" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="nav-accordion-three-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-three" type="button" role="tab" aria-controls="nav-accordion-three" aria-selected="false" tabindex="-1">Celebrations</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="nav-accordion-four-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-four" type="button" role="tab" aria-controls="nav-accordion-four" aria-selected="true">Rewards</button>
                        </li>
                        <!-- <li class="nav-item" role="presentation">
                            <button class="nav-link" id="nav-accordion-five-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-five" type="button" role="tab" aria-controls="nav-accordion-five" aria-selected="true"> East Coast Railway</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="nav-accordion-six-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-six" type="button" role="tab" aria-controls="nav-accordion-six" aria-selected="true">Eastern Railway</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="nav-accordion-seven-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-seven" type="button" role="tab" aria-controls="nav-accordion-seven" aria-selected="true">Central Railway</button>
                        </li> -->


                    </ul>
                </div>
                <!-- tab -->
                <div class="tab-content" id="nav-tabContent">
                    <div class="tab-pane fade faqPage active show" id="nav-accordion-three" role="tabpanel" aria-labelledby="nav-accordion-three-tab">

                        <ul class="station-tab nav nav-tabs border-bottom-0 d-flex align-items-center justify-content-center gap-3" style="margin-bottom:60px" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="nav-accordion-state-1-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-state-1" type="button" role="tab" aria-controls="nav-accordion-state-1" aria-selected="false" tabindex="-1">Republic Day</button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="nav-accordion-state-2-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-state-2" type="button" role="tab" aria-controls="nav-accordion-state-2" aria-selected="true">Independence Day</button>
                            </li>


                        </ul>
                        <div class="tab-content" id="nav-tabContent-state">
                            <div class="state-carousel-1 tab-pane fade faqPage active show" id="nav-accordion-state-1" role="tabpanel" aria-labelledby="nav-accordion-state-1-tab">
                                <div id="carouselExampleIndicators" class="carousel slide" data-bs-ride="true">
                                    <div class="carousel-indicators">
                                        <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                    </div>
                                    <div class="carousel-inner">
                                        <div class="carousel-item active">
                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="carousel-item">

                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="carousel-item">

                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
                                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Previous</span>
                                    </button>
                                    <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
                                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Next</span>
                                    </button>
                                </div>
                            </div>
                            <div class="state-carousel-1 tab-pane fade faqPage" id="nav-accordion-state-2" role="tabpanel" aria-labelledby="nav-accordion-state-2-tab">
                                <div id="carouselExampleIndicators2" class="carousel slide" data-bs-ride="true">
                                    <div class="carousel-indicators">
                                        <button type="button" data-bs-target="#carouselExampleIndicators2" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators2" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators2" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                    </div>
                                    <div class="carousel-inner">
                                        <div class="carousel-item active">
                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="carousel-item">
                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>

                                        </div>
                                        <div class="carousel-item">

                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators2" data-bs-slide="prev">
                                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Previous</span>
                                    </button>
                                    <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators2" data-bs-slide="next">
                                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Next</span>
                                    </button>
                                </div>


                            </div>
                        </div>

                    </div>

                    <div class="tab-pane fade faqPage" id="nav-accordion-four" role="tabpanel" aria-labelledby="nav-accordion-four-tab">
                        <!-- tab  -->
                        <ul class="station-tab nav nav-tabs border-bottom-0 d-flex align-items-center justify-content-center gap-3" style="margin-bottom:60px" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="nav-accordion-state-3-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-state-3" type="button" role="tab" aria-controls="nav-accordion-state-3" aria-selected="false" tabindex="-1">Reward Ceremony</button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="nav-accordion-state-4-tab" data-bs-toggle="tab" data-bs-target="#nav-accordion-state-4" type="button" role="tab" aria-controls="nav-accordion-state-4" aria-selected="true">General</button>
                            </li>


                        </ul>
                        <div class="tab-content" id="nav-tabContent-state">
                            <div class="state-carousel-1 tab-pane fade faqPage active show" id="nav-accordion-state-3" role="tabpanel" aria-labelledby="nav-accordion-state-3-tab">
                                <div id="carouselExampleIndicators3" class="carousel slide" data-bs-ride="true">
                                    <div class="carousel-indicators">
                                        <button type="button" data-bs-target="#carouselExampleIndicators3" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators3" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators3" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                    </div>
                                    <div class="carousel-inner">
                                        <div class="carousel-item active">
                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="carousel-item">
                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>

                                        </div>
                                        <div class="carousel-item">

                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators3" 3 data-bs-slide="prev">
                                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Previous</span>
                                    </button>
                                    <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators3" data-bs-slide="next">
                                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Next</span>
                                    </button>
                                </div>
                            </div>
                            <div class="state-carousel-1 tab-pane fade faqPage" id="nav-accordion-state-4" role="tabpanel" aria-labelledby="nav-accordion-state-4-tab">
                                <div id="carouselExampleIndicators4" class="carousel slide" data-bs-ride="true">
                                    <div class="carousel-indicators">
                                        <button type="button" data-bs-target="#carouselExampleIndicators4" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators4" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                        <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                    </div>
                                    <div class="carousel-inner">
                                        <div class="carousel-item active">
                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="carousel-item">

                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="carousel-item">

                                            <div class="row mb-20">
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/618x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                                <div class="col-sm-6">
                                                    <img class="w-100" src="images/558x332.png" alt="img" loading="lazy" style="height: 332px;">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                                <div class="col-sm-4">
                                                    <img class="w-100" src="images/386x332.png" alt="img" loading="lazy">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators4" data-bs-slide="prev">
                                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Previous</span>
                                    </button>
                                    <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators4" data-bs-slide="next">
                                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Next</span>
                                    </button>
                                </div>


                            </div>
                        </div>
                        <!-- tab  end-->
                    </div>

                    <!-- <div class="tab-pane fade faqPage" id="nav-accordion-five" role="tabpanel" aria-labelledby="nav-accordion-five-tab">
                      5
                    </div>
                    <div class="tab-pane fade faqPage" id="nav-accordion-six" role="tabpanel" aria-labelledby="nav-accordion-six-tab">
                      6
                    </div>
                    <div class="tab-pane fade faqPage" id="nav-accordion-seven" role="tabpanel" aria-labelledby="nav-accordion-seven-tab">
                      7
                    </div>
                     -->


                </div>


            </div>
        </section>

</asp:Content>

