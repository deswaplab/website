using DeswapApp;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Nethereum.Blazor;
using Nethereum.Metamask;
using Nethereum.Metamask.Blazor;
using Nethereum.UI;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IMetamaskInterop, MetamaskBlazorInterop>();
builder.Services.AddSingleton<MetamaskHostProvider>();
//Add metamask as the selected ethereum host provider
builder.Services.AddSingleton(services =>
{
    var metamaskHostProvider = services.GetService<MetamaskHostProvider>();
    var selectedHostProvider = new SelectedEthereumHostProviderService();
    selectedHostProvider.SetSelectedEthereumHostProvider(metamaskHostProvider);
    return selectedHostProvider;
});
builder.Services.AddSingleton<Web3Service>();
builder.Services.AddScoped<INftFetcher, OpenseaNftFetcher>();
builder.Services.AddScoped<PriceService>();

builder.Services.AddSingleton<AuthenticationStateProvider, EthereumAuthenticationStateProvider>();
builder.Services.AddSingleton<ToastService>();

builder.Services.AddLocalization();

var host = builder.Build();

CultureInfo culture;
var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");

if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo("en-US");
    await js.InvokeVoidAsync("blazorCulture.set", "en-US");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
