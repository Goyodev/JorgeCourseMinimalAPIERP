using Domain.Dtos;
using ERPUI;
using FrontEndERP.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IApiService<ProductDto>, ApiService<ProductDto>>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com/products");
});

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
