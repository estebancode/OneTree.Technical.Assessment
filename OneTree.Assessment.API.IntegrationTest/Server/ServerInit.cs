using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace OneTree.Assessment.API.IntegrationTest.Server
{
    public static class ServerInit
    {
        public static TestServer GetServer()
        {
            var projectDir = System.IO.Directory.GetCurrentDirectory();
            return new TestServer(new WebHostBuilder()
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.json")
                .Build())
                .UseStartup<Startup>());
        }
    }
}
