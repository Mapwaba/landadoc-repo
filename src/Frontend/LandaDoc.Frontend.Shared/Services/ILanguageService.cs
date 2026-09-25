namespace LandaDoc.Frontend.Shared.Services;

public interface ILanguageService
{
    string CurrentLanguage { get; }
    Task InitializeAsync();
    Task SetLanguageAsync(string language);
    string T(string key);
    string this[string key] { get; }
}
