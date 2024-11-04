using Elastic.Apm.AspNetCore;
using MudBlazor.Services;
using Whiskey_TastingTale_Frontend.Services;
using Whiskey_TastingTale_Frontend.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<SearchViewModel>();
builder.Services.AddScoped<DetailsViewModel>();
builder.Services.AddScoped<MyPageViewModel>();
builder.Services.AddScoped<RegisterViewModel>();
builder.Services.AddScoped<AddWhiskeyViewModel>();
builder.Services.AddScoped<RequestListViewModel>();
builder.Services.AddScoped<UserManagementViewModel>();
builder.Services.AddScoped<RequestManagementViewMdoel>();
builder.Services.AddScoped<WhiskeyManagementViewMdoel>();
builder.Services.AddScoped<ReviewManagementViewMdoel>(); 
builder.Services.AddScoped<NotificationViewModel>();


builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<PageState>();
builder.Services.AddScoped<SelectState>();
builder.Services.AddScoped<RestApiHelper>();
builder.Services.AddSignalR();

builder.Services.AddSingleton(new CustomMudTheme());

builder.Services.AddElasticApm();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    });

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 8001; // HTTPS 포트 설정
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseElasticApm();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
