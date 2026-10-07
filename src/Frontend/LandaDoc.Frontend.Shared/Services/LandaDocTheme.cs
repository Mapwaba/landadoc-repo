using MudBlazor;

namespace LandaDoc.Frontend.Shared.Services;

// Shared look for the Patient, Doctor and Admin web apps: white surfaces on a light grey
// background, one blue accent, and the Inter font (loaded in each app's index.html).
// The layout shell itself is AppShell.razor + wwwroot/landadoc-shell.css.
public static class LandaDocTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2563EB",
            Secondary = "#64748B",
            Success = "#16A34A",
            Warning = "#F59E0B",
            Error = "#DC2626",
            Background = "#F5F7FB",
            Surface = "#FFFFFF",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#1E293B",
            TextPrimary = "#1E293B",
            TextSecondary = "#64748B",
            LinesDefault = "#E5E7EB",
            TableLines = "#EEF0F4",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "Helvetica Neue", "Helvetica", "Arial", "sans-serif"],
            },
            Button = new ButtonTypography
            {
                FontFamily = ["Inter", "Helvetica Neue", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "600",
                TextTransform = "none",
            },
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "10px",
        },
    };
}
