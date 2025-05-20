using Serilog;

namespace LangAI.API.Configuration;
public static class SerilogConfig
{
    public static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) =>
        {
            lc.ReadFrom.Configuration(ctx.Configuration)
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .WriteTo.File("Logs/LangAI.log", rollingInterval: RollingInterval.Day);
        });
    }
}