/* Accessible display controls for the site-wide master page. */
(function () {
    'use strict';

    var storageKey = 'marreg-accessibility-settings';
    var defaults = {
        fontScale: 0,
        lineHeight: 0,
        textSpacing: 0,
        screenReader: false,
        highlightLinks: false,
        dyslexia: false,
        hideImages: false,
        cursor: false,
        theme: 'light',
        inverted: false
    };

    var settings = loadSettings();
    var root = document.documentElement;
    var panel = document.getElementById('uw-main');
    function loadSettings() {
        try {
            return Object.assign({}, defaults, JSON.parse(localStorage.getItem(storageKey) || '{}'));
        } catch (error) {
            return Object.assign({}, defaults);
        }
    }

    function saveSettings() {
        localStorage.setItem(storageKey, JSON.stringify(settings));
    }

    function feature(id, enabled) {
        var item = document.getElementById(id);
        var button = item && item.querySelector('button');
        var tick = item && item.querySelector('.reset-tick');
        if (item) item.classList.toggle('feature-active', Boolean(enabled));
        if (button) button.setAttribute('aria-pressed', String(Boolean(enabled)));
        if (tick) tick.style.display = enabled ? 'inline-flex' : 'none';
    }

    function levelFeature(id, level, steps) {
        feature(id, level > 0);
        var container = document.querySelector('#' + id + ' .reset-steps');
        if (!container) return;
        container.classList.toggle('featureSteps-visible', level > 0);
        container.innerHTML = '';
        for (var i = 0; i < steps; i += 1) {
            var step = document.createElement('span');
            step.className = 'step uwaw-features__step' + (i < level ? ' active' : '');
            container.appendChild(step);
        }
    }

    function applySettings() {
        root.style.setProperty('--a11y-font-scale', String(settings.fontScale));
        root.style.setProperty('--a11y-line-height', String(settings.lineHeight));
        root.style.setProperty('--a11y-text-spacing', String(settings.textSpacing));
        root.classList.toggle('a11y-highlight-links', settings.highlightLinks);
        root.classList.toggle('a11y-dyslexia', settings.dyslexia);
        root.classList.toggle('a11y-hide-images', settings.hideImages);
        root.classList.toggle('a11y-large-cursor', settings.cursor);
        root.classList.toggle('a11y-inverted', settings.inverted);
        document.body.classList.toggle('dark', settings.theme === 'dark');

        var checkbox = document.getElementById('checkbox');
        if (checkbox) checkbox.checked = settings.theme === 'dark';
        updateTextControls();

        levelFeature('featureItem', settings.fontScale, 3);
        levelFeature('featureItem-st', -settings.fontScale, 3);
        levelFeature('featureItem-lh', settings.lineHeight, 3);
        levelFeature('featureItem-ts', settings.textSpacing, 3);
        feature('featureItem_sp', settings.screenReader);
        feature('featureItem-ht', settings.highlightLinks);
        feature('featureItem-df', settings.dyslexia);
        feature('featureItem-hi', settings.hideImages);
        feature('featureItem-Cursor', settings.cursor);
        feature('featureItem-ht-dark', settings.theme === 'dark');
        feature('featureItem-ic', settings.inverted);
    }

    function update(callback) {
        callback();
        saveSettings();
        applySettings();
    }

    function updateTextControls() {
        var resetButton = document.getElementById('btn-orig');
        if (resetButton) resetButton.classList.toggle('active01', settings.fontScale === 0);
    }

    function changeFontSize(amount) {
        update(function () {
            settings.fontScale = Math.max(-3, Math.min(3, settings.fontScale + amount));
        });
    }

    function speakPage() {
        if (!('speechSynthesis' in window)) return;
        window.speechSynthesis.cancel();
        if (!settings.screenReader) return;
        var main = document.querySelector('main, [role="main"], #ContentPlaceHolder1') || document.body;
        var utterance = new SpeechSynthesisUtterance(main.innerText.replace(/\s+/g, ' ').trim());
        window.speechSynthesis.speak(utterance);
    }

    function bind(id, callback) {
        var button = document.getElementById(id);
        if (button) button.addEventListener('click', callback);
    }

    bind('btn-s9', function () { changeFontSize(1); });
    bind('btn-small-text', function () { changeFontSize(-1); });
    bind('btn-increase', function () { changeFontSize(1); });
    bind('btn-decrease', function () { changeFontSize(-1); });
    bind('btn-orig', function () { update(function () { settings.fontScale = defaults.fontScale; }); });
    bind('btn-s12', function () { update(function () { settings.lineHeight = settings.lineHeight >= 3 ? 0 : settings.lineHeight + 1; }); });
    bind('btn-s13', function () { update(function () { settings.textSpacing = settings.textSpacing >= 3 ? 0 : settings.textSpacing + 1; }); });
    bind('btn-s10', function () { update(function () { settings.highlightLinks = !settings.highlightLinks; }); });
    bind('btn-df', function () { update(function () { settings.dyslexia = !settings.dyslexia; }); });
    bind('btn-s11', function () { update(function () { settings.hideImages = !settings.hideImages; }); });
    bind('btn-cursor', function () { update(function () { settings.cursor = !settings.cursor; }); });
    bind('btn-invert', function () { update(function () { settings.inverted = !settings.inverted; }); });
    bind('speak', function () { update(function () { settings.screenReader = !settings.screenReader; }); speakPage(); });

    var themeToggle = document.getElementById('checkbox');
    if (themeToggle) themeToggle.addEventListener('change', function () {
        update(function () { settings.theme = themeToggle.checked ? 'dark' : 'light'; });
    });
    bind('dark-btn', function (event) {
        if (event.target !== themeToggle) update(function () { settings.theme = settings.theme === 'dark' ? 'light' : 'dark'; });
    });

    window.openMain = function () { if (panel) { panel.hidden = false; panel.classList.add('is-open'); panel.querySelector('button') && panel.querySelector('button').focus(); } };
    window.closeMain = function () { if (panel) panel.classList.remove('is-open'); };
    window.resetAll = function () {
        if ('speechSynthesis' in window) window.speechSynthesis.cancel();
        settings = Object.assign({}, defaults);
        saveSettings();
        applySettings();
    };

    bind('uw-widget-custom-trigger', function (event) { event.preventDefault(); window.openMain(); });
    applySettings();
}());