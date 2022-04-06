using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore;
using Winton.Extensions.Configuration.Consul;

namespace ConfigurationWithConsul
{
	public class Program
	{
		public static void Main(string[] args)
		{
            var initConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

            //var cancellationTokenSource = new CancellationTokenSource();
            var x = initConfig["Consul:Host"];
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (hostingContext, builder) =>
                    {
                        builder
                            .AddConsul(
                                "App1/appsettings.json",
                                //cancellationTokenSource.Token,
                                options =>
                                {
                                    options.ConsulConfigurationOptions =
                                        cco => { cco.Address = new Uri(initConfig["Consul:Host"]); };
                                    options.Optional = true;
                                    options.ReloadOnChange = true;
                                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                                })
                            .AddEnvironmentVariables();
                    })
                .ConfigureLogging(c =>
                {
                    c.AddSerilog(Log.Logger);
                })
                .UseSerilog(Log.Logger)
                .UseStartup<Startup>()
                .Build()
                .Run();
            //cancellationTokenSource.Cancel();
		}
	}
}
