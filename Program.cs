    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using TodoApp.Client;
    using TodoApp.Client.Services;

    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped(sp =>
        new HttpClient { BaseAddress = new Uri("https://localhost:7175/") });

    builder.Services.AddScoped<AuthHttpHandler>();
    builder.Services.AddHttpClient("AuthorizedClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7175/");
        })
        .AddHttpMessageHandler<AuthHttpHandler>();

    await builder.Build().RunAsync();

