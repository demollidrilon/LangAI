using LangAI.API.Middleware;
using LangAI.API.Configuration;
using LangAI.Application.Options;
using LangAI.Application.Interfaces;
using LangAI.Infrastructure.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

SerilogConfig.ConfigureLogging(builder);
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAIConfigs"));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { builder.Configuration.GetConnectionString("Redis") },
        ConnectTimeout = 500,
        SyncTimeout = 500,
        AbortOnConnectFail = false,
        ConnectRetry = 1,
    };
});
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ITranslationService, TranslationService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();