using Auth0IdentityCoreApp.Client.Pages;
using Auth0IdentityCoreApp.Components;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services
    .AddAuth0WebAppAuthentication(options => {
      options.Domain = builder.Configuration["Auth0:Domain"];
      options.ClientId = builder.Configuration["Auth0:ClientId"];
      options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];

      options.AddTracing();
    })
    .WithAccessToken(options =>
    {
        options.Audience = builder.Configuration["Auth0:Audience"];
    });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();

builder.Services.AddHttpClient("ExternalAPI", 
      client => client.BaseAddress = new Uri(builder.Configuration["ExternalApiBaseUrl"]))
      .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("ExternalAPI"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/account/login", async (HttpContext httpContext, string redirectUri = "/") =>
{
  var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
          .WithRedirectUri(redirectUri)
          .Build();

  await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/account/logout", async (HttpContext httpContext, string redirectUri = "/") =>
{
  var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
          .WithRedirectUri(redirectUri)
          .Build();

  await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
  await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapGet("/api/internalData", () =>
{
    var startDate = DateOnly.FromDateTime(DateTime.Now);
    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = startDate.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = summaries[Random.Shared.Next(summaries.Length)]
    }).ToArray();

    return forecasts;
})
.RequireAuthorization();

app.MapGet("/api/externalData", async (HttpClient httpClient) =>
{
    return await httpClient.GetFromJsonAsync<WeatherForecast[]>("data");
})
.RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Auth0IdentityCoreApp.Client._Imports).Assembly);

app.Run();
