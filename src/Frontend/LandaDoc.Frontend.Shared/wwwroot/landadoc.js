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

// Branded A4 PDF reports (Components/PdfButton.razor builds the content, Services/PdfReport.cs
// describes its shape). jsPDF and its table plugin live in lib/ and are only loaded the first
// time someone asks for a PDF.
window.landadoc.pdf = (function () {
    const base = "_content/LandaDoc.Frontend.Shared/";
    const libs = [base + "lib/jspdf.umd.min.js", base + "lib/jspdf.plugin.autotable.min.js"];
    const logoUrl = base + "landadoc-logo.png";

    // Same palette as landadoc-shell.css
    const primary = [37, 99, 235];
    const primarySoft = [238, 244, 255];
    const primaryDark = [30, 64, 175];
    const text = [30, 41, 59];
    const muted = [100, 116, 139];
    const border = [229, 231, 235];
    const tint = [245, 247, 251];
    const stripe = [250, 251, 253];
    // Status badges, as .status-pill.status-* (background, text)
    const pills = {
        confirmed: [[220, 252, 231], [21, 128, 61]],
        pending: [[254, 243, 199], [180, 83, 9]],
        completed: [[219, 234, 254], [29, 78, 216]],
        rescheduled: [[237, 233, 254], [109, 40, 217]],
        cancelled: [[243, 244, 246], [107, 114, 128]],
        failed: [[254, 226, 226], [185, 28, 28]],
        refunded: [[237, 233, 254], [109, 40, 217]]
    };

    const pageW = 210, pageH = 297, margin = 16;
    const contentW = pageW - 2 * margin;
    const topOfNextPages = 26;      // below the small header drawn on pages 2+
    const bottomLimit = pageH - 22; // above the footer

    let loading = null;
    let logo = null;

    function loadScript(src) {
        return new Promise(function (resolve, reject) {
            const s = document.createElement("script");
            s.src = src;
            s.onload = resolve;
            s.onerror = function () { reject(new Error("Couldn't load " + src)); };
            document.head.appendChild(s);
        });
    }

    function ensureLibs() {
        if (!loading) {
            loading = loadScript(libs[0])
                .then(function () { return loadScript(libs[1]); })
                .catch(function (e) { loading = null; throw e; });
        }
        return loading;
    }

    // The logo as a data URL plus its width/height ratio; null if it can't be fetched
    async function loadLogo() {
        if (logo) return logo;
        try {
            const blob = await (await fetch(logoUrl)).blob();
            const dataUrl = await new Promise(function (resolve, reject) {
                const r = new FileReader();
                r.onload = function () { resolve(r.result); };
                r.onerror = reject;
                r.readAsDataURL(blob);
            });
            const img = await new Promise(function (resolve, reject) {
                const i = new Image();
                i.onload = function () { resolve(i); };
                i.onerror = reject;
                i.src = dataUrl;
            });
            logo = { dataUrl: dataUrl, ratio: img.naturalWidth / img.naturalHeight };
        } catch (e) {
            logo = null;
        }
        return logo;
    }

    // The built-in PDF fonts only cover Latin-1: swap the usual typographic characters for
    // plain ones (dashes, quotes, the narrow spaces .NET puts in French numbers and dates).
    function clean(value) {
        return String(value == null ? "" : value)
            .replace(/[–—]/g, "-")
            .replace(/[‘’]/g, "'")
            .replace(/[“”]/g, "\"")
            .replace(/…/g, "...")
            .replace(/€/g, "EUR")
            .replace(/[ -​  ]/g, " ")
            .replace(/[^\x00-\xFF]/g, "?");
    }

    function pillOf(cell) {
        if (!cell || !cell.pill) return null;
        return pills[String(cell.pill).replace(/^status-/, "")] || pills.cancelled;
    }

    function setFont(doc, size, style, color) {
        doc.setFont("helvetica", style || "normal");
        doc.setFontSize(size);
        doc.setTextColor(color[0], color[1], color[2]);
    }

    function fill(doc, color) {
        doc.setFillColor(color[0], color[1], color[2]);
    }

    // Start a new page when fewer than `needed` mm are left
    function room(doc, y, needed) {
        if (y + needed <= bottomLimit) return y;
        doc.addPage();
        return topOfNextPages;
    }

    // Logo top-left, who the report is for top-right, then a blue rule and the title block
    function drawHeader(doc, report, logoImg) {
        if (logoImg) {
            const h = 14;
            doc.addImage(logoImg.dataUrl, "PNG", margin, 12, h * logoImg.ratio, h);
        } else {
            setFont(doc, 18, "bold", primary);
            doc.text("LandaDoc", margin, 22);
        }
        let ay = 15.5;
        (report.author || []).forEach(function (line, i) {
            setFont(doc, i === 0 ? 10 : 8.5, i === 0 ? "bold" : "normal", i === 0 ? text : muted);
            doc.text(clean(line), pageW - margin, ay, { align: "right" });
            ay += i === 0 ? 5 : 4.2;
        });

        fill(doc, primary);
        doc.rect(margin, 32, contentW, 0.9, "F");

        let y = 44;
        setFont(doc, 20, "bold", text);
        doc.splitTextToSize(clean(report.title), contentW).forEach(function (line) {
            doc.text(line, margin, y);
            y += 8;
        });
        y -= 1.5;
        if (report.subtitle) {
            setFont(doc, 10.5, "normal", muted);
            doc.text(clean(report.subtitle), margin, y);
            y += 5.5;
        }
        setFont(doc, 8.5, "normal", muted);
        doc.text(clean(report.generatedOn), margin, y);
        return y + 9;
    }

    // Key figures as tinted tiles, up to five per row
    function drawStats(doc, stats, y) {
        if (!stats || stats.length === 0) return y;
        const perRow = stats.length <= 5 ? stats.length : 4;
        const gap = 4, h = 21;
        const w = (contentW - gap * (perRow - 1)) / perRow;
        for (let i = 0; i < stats.length; i += perRow) {
            y = room(doc, y, h + 4);
            stats.slice(i, i + perRow).forEach(function (s, j) {
                const x = margin + j * (w + gap);
                fill(doc, tint);
                doc.roundedRect(x, y, w, h, 2.5, 2.5, "F");
                fill(doc, primary);
                doc.rect(x, y + 4, 1, h - 8, "F");
                setFont(doc, 7, "bold", muted);
                doc.text(doc.splitTextToSize(clean(s.label).toUpperCase(), w - 8)[0], x + 5, y + 7.5);
                setFont(doc, 14, "bold", text);
                doc.text(doc.splitTextToSize(clean(s.value), w - 8)[0], x + 5, y + 15.5);
            });
            y += h + gap;
        }
        return y + 5;
    }

    function drawSectionTitle(doc, section, y) {
        y = room(doc, y, section.columns && section.columns.length ? 30 : 20);
        fill(doc, primary);
        doc.rect(margin, y - 4, 1.2, 5.2, "F");
        setFont(doc, 12, "bold", text);
        doc.text(clean(section.title), margin + 4, y);
        y += 3;
        if (section.note) {
            setFont(doc, 8.5, "normal", muted);
            const lines = doc.splitTextToSize(clean(section.note), contentW);
            doc.text(lines, margin, y + 3.5);
            y += 4 * lines.length + 1;
        }
        return y + 4;
    }

    // Label/value pairs, two per row
    function drawDetails(doc, details, y) {
        if (!details || details.length === 0) return y;
        const colW = contentW / 2;
        for (let i = 0; i < details.length; i += 2) {
            y = room(doc, y, 12);
            details.slice(i, i + 2).forEach(function (d, j) {
                const x = margin + j * colW;
                setFont(doc, 7, "bold", muted);
                doc.text(clean(d.label).toUpperCase(), x, y + 3);
                setFont(doc, 10, "normal", text);
                doc.text(doc.splitTextToSize(clean(d.value), colW - 6)[0], x, y + 8);
            });
            y += 12;
        }
        return y + 8;
    }

    function drawEmpty(doc, message, y) {
        y = room(doc, y, 16);
        fill(doc, tint);
        doc.roundedRect(margin, y, contentW, 13, 2, 2, "F");
        setFont(doc, 9, "italic", muted);
        doc.text(clean(message || "-"), pageW / 2, y + 8, { align: "center" });
        return y + 21;
    }

    function drawTable(doc, section, y) {
        const columns = section.columns;
        const columnStyles = {};
        columns.forEach(function (c, i) { if (c.right) columnStyles[i] = { halign: "right" }; });
        const texts = function (cells) { return cells.map(function (c) { return clean(c && c.text); }); };

        doc.autoTable({
            startY: y,
            margin: { left: margin, right: margin, top: topOfNextPages, bottom: pageH - bottomLimit },
            head: [columns.map(function (c) { return clean(c.header); })],
            body: section.rows.map(texts),
            foot: section.total ? [texts(section.total)] : undefined,
            showFoot: "lastPage",
            showHead: "everyPage",
            theme: "plain",
            columnStyles: columnStyles,
            styles: {
                font: "helvetica",
                fontSize: 8.5,
                textColor: text,
                cellPadding: { top: 2.8, bottom: 2.8, left: 3, right: 3 },
                lineColor: border,
                lineWidth: { bottom: 0.2 },
                valign: "middle",
                overflow: "linebreak"
            },
            headStyles: { fillColor: primarySoft, textColor: primaryDark, fontStyle: "bold", fontSize: 7.5, lineWidth: 0 },
            footStyles: { fillColor: tint, textColor: text, fontStyle: "bold", lineWidth: 0 },
            alternateRowStyles: { fillColor: stripe },
            didParseCell: function (data) {
                const col = columns[data.column.index];
                if (data.section !== "body" && col && col.right) data.cell.styles.halign = "right";
                if (data.section !== "body") return;
                const pill = pillOf(section.rows[data.row.index][data.column.index]);
                if (pill) {
                    data.cell.styles.textColor = pill[1];
                    data.cell.styles.fontStyle = "bold";
                    data.cell.styles.fontSize = 7;
                }
            },
            // Status cells: a rounded badge behind the text, like the app's status pills
            willDrawCell: function (data) {
                if (data.section !== "body") return;
                const pill = pillOf(section.rows[data.row.index][data.column.index]);
                if (!pill) return;
                const cell = data.cell;
                // Paint the row background first, then the badge on top of it
                if (cell.styles.fillColor) {
                    fill(doc, cell.styles.fillColor);
                    doc.rect(cell.x, cell.y, cell.width, cell.height, "F");
                    cell.styles.fillColor = false;
                }
                doc.setFont("helvetica", "bold");
                doc.setFontSize(7);
                const w = doc.getTextWidth((cell.text || []).join(" ")) + 5;
                const h = 5;
                const x = cell.styles.halign === "right"
                    ? cell.x + cell.width - cell.padding("right") - w + 2.5
                    : cell.x + cell.padding("left") - 2.5;
                fill(doc, pill[0]);
                doc.roundedRect(x, cell.y + (cell.height - h) / 2, w, h, 2.5, 2.5, "F");
            }
        });
        return doc.lastAutoTable.finalY + 10;
    }

    // Small header on pages 2+, footer with page numbers on every page
    function drawPageFrames(doc, report, logoImg) {
        const count = doc.internal.getNumberOfPages();
        for (let i = 1; i <= count; i++) {
            doc.setPage(i);
            doc.setDrawColor(border[0], border[1], border[2]);
            doc.setLineWidth(0.3);
            if (i > 1) {
                if (logoImg) doc.addImage(logoImg.dataUrl, "PNG", margin, 9, 7.5 * logoImg.ratio, 7.5);
                setFont(doc, 8.5, "bold", muted);
                doc.text(clean(report.title), pageW - margin, 14.5, { align: "right" });
                doc.line(margin, 19, pageW - margin, 19);
            }
            doc.line(margin, pageH - 15, pageW - margin, pageH - 15);
            setFont(doc, 7.5, "normal", muted);
            doc.text(clean(report.footer), margin, pageH - 10);
            const page = String(report.pageFormat || "{0} / {1}").replace("{0}", i).replace("{1}", count);
            doc.text(clean(page), pageW - margin, pageH - 10, { align: "right" });
        }
    }

    return {
        download: async function (report) {
            await ensureLibs();
            const logoImg = await loadLogo();
            const doc = new window.jspdf.jsPDF({ unit: "mm", format: "a4" });
            doc.setProperties({ title: clean(report.title), creator: "LandaDoc" });

            let y = drawHeader(doc, report, logoImg);
            y = drawStats(doc, report.stats, y);
            (report.sections || []).forEach(function (section) {
                y = drawSectionTitle(doc, section, y);
                y = drawDetails(doc, section.details, y);
                if (section.columns && section.columns.length) {
                    y = section.rows && section.rows.length
                        ? drawTable(doc, section, y)
                        : drawEmpty(doc, section.emptyText, y);
                }
            });

            drawPageFrames(doc, report, logoImg);
            doc.save(report.fileName || "landadoc-report.pdf");
        }
    };
})();
