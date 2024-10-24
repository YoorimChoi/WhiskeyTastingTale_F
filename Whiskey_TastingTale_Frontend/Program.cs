using MudBlazor.Services;

using Whiskey_TastingTale_Frontend.Services;
using Whiskey_TastingTale_Frontend.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddSingleton<SearchViewModel>();
builder.Services.AddSingleton<DetailsViewModel>();
builder.Services.AddSingleton<MyPageViewModel>();
builder.Services.AddSingleton<RegisterViewModel>();
builder.Services.AddSingleton<AddWhiskeyViewModel>();
builder.Services.AddSingleton<RequestListViewModel>();
builder.Services.AddSingleton<UserManagementViewModel>();
builder.Services.AddSingleton<RequestManagementViewMdoel>();
builder.Services.AddSingleton<WhiskeyManagementViewMdoel>();
builder.Services.AddSingleton<ReviewManagementViewMdoel>(); 

builder.Services.AddSingleton<UserState>();
builder.Services.AddSingleton<PageState>();
builder.Services.AddSingleton<WhiskeyState>();
builder.Services.AddSingleton<RestApiHelper>();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    });


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
