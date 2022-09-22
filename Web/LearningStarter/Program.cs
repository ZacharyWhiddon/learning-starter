using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LearningStarter;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder
                .CaptureStartupErrors(true)
                .UseStartup<Startup>());
    }
}
