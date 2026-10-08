namespace LandaDoc.Frontend.Shared.Services;

// A report a page hands to Components/PdfButton.razor, which turns it into a branded A4 PDF
// (drawn by landadoc.pdf in wwwroot/landadoc.js). Pages describe the content only: title,
// key figures and sections of tables or label/value details. The header (logo, who it's for,
// date) and the footer (page numbers) are added for them.
public sealed class PdfReport
{
    // Without ".pdf"; the date is appended, e.g. "landadoc-payments-2026-10-08.pdf"
    public string FileName { get; set; } = "landadoc-report";
    public string Title { get; set; } = "";
    public string? Subtitle { get; set; }
    // Key figures shown as tiles under the title
    public List<PdfStat> Stats { get; set; } = [];
    public List<PdfSection> Sections { get; set; } = [];

    // Filled in by PdfButton
    public List<string> Author { get; set; } = [];
    public string GeneratedOn { get; set; } = "";
    public string Footer { get; set; } = "";
    public string PageFormat { get; set; } = "Page {0} / {1}";
}

public sealed record PdfStat(string Label, string Value);

public sealed class PdfSection
{
    public string Title { get; set; } = "";
    public string? Note { get; set; }
    // Label/value pairs in two columns, e.g. a patient's details
    public List<PdfStat> Details { get; set; } = [];
    // A table; leave Columns empty for a details-only section
    public List<PdfColumn> Columns { get; set; } = [];
    public List<List<PdfCell>> Rows { get; set; } = [];
    // Bold last row, e.g. totals; one cell per column
    public List<PdfCell>? Total { get; set; }
    // Shown instead of the table when there are no rows
    public string? EmptyText { get; set; }
}

public sealed record PdfColumn(string Header, bool Right = false);

// A table cell. Pill = a status colour as used by the .status-pill CSS classes
// ("confirmed", "pending", "completed", "rescheduled", "cancelled", "failed", "refunded"),
// with or without the "status-" prefix; the text is then drawn as a coloured badge.
public sealed record PdfCell(string Text, string? Pill = null)
{
    public static implicit operator PdfCell(string text) => new(text);
}
