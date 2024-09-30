using MudBlazor.Services;

using Whiskey_TastingTale_Frontend.Services;
using Whiskey_TastingTale_Frontend.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddSingleton<SearchViewModel>();
builder.Services.AddScoped<DetailsViewModel>();
builder.Services.AddSingleton<UserState>();
builder.Services.AddSingleton<WhiskeyState>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
