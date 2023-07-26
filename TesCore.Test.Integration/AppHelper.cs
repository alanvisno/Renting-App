using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TesCore.Test.Integration
{
    public static class AppHelper
    {
        public static WebApplicationFactory<Program> GetAppWithSettings() 
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.json");
                    });
                });
        }
    }
}
