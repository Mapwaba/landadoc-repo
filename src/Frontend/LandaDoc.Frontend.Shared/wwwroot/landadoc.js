// Small browser helpers called from Blazor through IJSRuntime.
window.landadoc = window.landadoc || {};

// Saves text as a file, e.g. the Doctor app's payments CSV export. The BOM makes Excel read UTF-8.
window.landadoc.downloadText = function (fileName, text, mimeType) {
    const blob = new Blob(["﻿" + text], { type: mimeType });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    link.remove();
    setTimeout(function () { URL.revokeObjectURL(url); }, 1000);
};

// Stamps the print header (AppShell.razor) with the time of printing, whether printing starts from
// a Print button or the browser's own Ctrl+P. Blazor renders the span empty and never touches it.
window.addEventListener("beforeprint", function () {
    const now = new Date();
    const pad = function (n) { return String(n).padStart(2, "0"); };
    const stamp = pad(now.getDate()) + "/" + pad(now.getMonth() + 1) + "/" + now.getFullYear()
        + " " + pad(now.getHours()) + ":" + pad(now.getMinutes());
    document.querySelectorAll(".ld-print-date").forEach(function (el) { el.textContent = stamp; });
});

// Inactivity watch for signed-in users (used by IdleSessionGuard.razor). Any mouse, keyboard,
// scroll or touch activity counts. The last activity time is shared through localStorage, so
// working in one tab keeps the others alive too. When nobody has been active for idleMs, .NET's
// OnIdle is called once (with how long it's been, in ms) and the guard shows its prompt; the
// watch then pauses until resume(). Times are compared as timestamps, so it stays correct even
// when the browser slows timers down in a background tab.
window.landadoc.idle = (function () {
    const storageKey = "landadoc-last-active";
    const activityEvents = ["mousemove", "mousedown", "keydown", "scroll", "touchstart", "wheel"];
    let dotnet = null;
    let idleMs = 0;
    let lastActive = 0;    // this tab's latest activity
    let lastShared = 0;    // when we last wrote to localStorage (written at most every 5 s)
    let prompting = false; // the prompt is showing: activity no longer counts until resume()
    let interval = null;

    function sharedLastActive() {
        try { return Number(localStorage.getItem(storageKey)) || 0; } catch { return 0; }
    }

    function markActive() {
        if (prompting) return;
        lastActive = Date.now();
        if (lastActive - lastShared > 5000) {
            lastShared = lastActive;
            try { localStorage.setItem(storageKey, String(lastActive)); } catch { /* private mode */ }
        }
    }

    function check() {
        if (!dotnet || prompting) return;
        const idleFor = Date.now() - Math.max(lastActive, sharedLastActive());
        if (idleFor >= idleMs) {
            prompting = true;
            dotnet.invokeMethodAsync("OnIdle", idleFor);
        }
    }

    function onVisibility() {
        if (document.visibilityState === "visible") check();
    }

    // Someone pressed "Continue" (or is simply working) in another tab: close our prompt too
    function onStorage(e) {
        if (e.key === storageKey && prompting && dotnet && Number(e.newValue) > lastActive) {
            prompting = false;
            lastActive = Number(e.newValue);
            dotnet.invokeMethodAsync("OnActiveElsewhere");
        }
    }

    return {
        start: function (dotnetRef, idleMilliseconds) {
            this.stop();
            dotnet = dotnetRef;
            idleMs = idleMilliseconds;
            prompting = false;
            lastShared = 0;
            markActive();
            activityEvents.forEach(function (name) { window.addEventListener(name, markActive, { passive: true }); });
            document.addEventListener("visibilitychange", onVisibility);
            window.addEventListener("storage", onStorage);
            interval = setInterval(check, 10000);
        },
        // The user chose to continue: count from now again
        resume: function () {
            prompting = false;
            lastShared = 0;
            markActive();
        },
        stop: function () {
            activityEvents.forEach(function (name) { window.removeEventListener(name, markActive); });
            document.removeEventListener("visibilitychange", onVisibility);
            window.removeEventListener("storage", onStorage);
            if (interval) clearInterval(interval);
            interval = null;
            dotnet = null;
        }
    };
})();
