using Elastic.Apm.AspNetCore;
using MudBlazor.Services;
using Whiskey_TastingTale_Frontend;
using Whiskey_TastingTale_Frontend.Services;
using Whiskey_TastingTale_Frontend.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); 


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


builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 8001; // HTTPS 포트 설정
});

var app = builder.Build();


app.UseElasticApm();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
