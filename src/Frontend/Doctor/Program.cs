using LandaDoc.Doctor;
using LandaDoc.Frontend.Shared;
using LandaDoc.Frontend.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLandaDocFrontendShared(builder.Configuration);
builder.Services.AddScoped<LandaDoc.Doctor.Services.DoctorApproval>();

var host = builder.Build();
await host.Services.GetRequiredService<ILanguageService>().InitializeAsync();
await host.RunAsync();
