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
