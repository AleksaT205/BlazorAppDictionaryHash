using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.Extensions.Logging;
using MyDictionaryWasmApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registruj MudBlazor servise
builder.Services.AddMudServices();

// Smanji logovanje za MudBlazor.PopoverService (da ne prikazuje greške za popover)
builder.Logging.AddFilter("MudBlazor.PopoverService", LogLevel.None);

// Tvoj HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
