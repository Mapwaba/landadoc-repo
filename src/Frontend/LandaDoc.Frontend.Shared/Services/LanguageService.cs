using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace LandaDoc.Frontend.Shared.Services;

// Language is resolved once at app boot (InitializeAsync, called from Program.cs before
// RunAsync) so every component's first render already has the right strings — no flash of
// English, no need for components to subscribe to a change event. Switching language does a
// full reload (see SetLanguageAsync) specifically to keep that same "resolved once" guarantee
// instead of pushing change-notification/StateHasChanged plumbing into every page.
public class LanguageService(ILocalStorageService localStorage, NavigationManager navigation) : ILanguageService
{
    private const string StorageKey = "landadoc_language";
    private const string DefaultLanguage = "en";

    public string CurrentLanguage { get; private set; } = DefaultLanguage;

    public async Task InitializeAsync()
    {
        var saved = await localStorage.GetItemAsStringAsync(StorageKey);
        if (saved is "en" or "fr") CurrentLanguage = saved;
        ApplyCulture();
    }

    // MudBlazor's own built-in strings (date/time picker toolbars, calendar month/day
    // names, etc.) come from its own culture-bound resources, not from Translations.Map —
    // without this they silently stay in English even after switching CurrentLanguage.
    private void ApplyCulture()
    {
        var culture = new CultureInfo(CurrentLanguage == "fr" ? "fr-FR" : "en-US");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public async Task SetLanguageAsync(string language)
    {
        if (language is not ("en" or "fr") || language == CurrentLanguage) return;
        await localStorage.SetItemAsStringAsync(StorageKey, language);
        navigation.NavigateTo(navigation.Uri, forceLoad: true);
    }

    public string T(string key)
    {
        if (!Translations.Map.TryGetValue(key, out var byLanguage)) return key;
        if (byLanguage.TryGetValue(CurrentLanguage, out var text)) return text;
        return byLanguage.GetValueOrDefault(DefaultLanguage, key);
    }

    public string this[string key] => T(key);
}
