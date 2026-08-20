using Kwerty.DviZe.Logging.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BasicUsageExampleApp1;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging
            .SetMinimumLevel(LogLevel.Debug)
            .AddDviZeConsole(consoleBuilder =>
            {
                consoleBuilder
                    .IncludeScopes(true)
                    .UseUtcTimestamp(true)
                    .UseTimestampFormat("HH:mm:ss")
                    .StripNamespace("BasicUsageExampleApp1", matchNamespaceExactly: false)
                    .UseForegroundColorForNamespace("Microsoft", DviZeConsoleColor.Cyan)
                    .ConfigureDefaultColors(new Dictionary<LogLevel, DviZeConsoleColorPair>()
                    {
                        [LogLevel.Trace] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
                        [LogLevel.Debug] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
                        [LogLevel.Information] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
                        [LogLevel.Warning] = new(DviZeConsoleColor.Black, DviZeConsoleColor.DarkYellow),
                        [LogLevel.Error] = new(DviZeConsoleColor.White, DviZeConsoleColor.DarkRed),
                        [LogLevel.Critical] = new(DviZeConsoleColor.White, DviZeConsoleColor.DarkRed),
                    });
            });

        builder.Services.AddHostedService<ExampleBackgroundService>();

        var host = builder.Build();

        // Restores legacy CTRL_CLOSE_EVENT handling on Windows, which was removed with .NET 10.
        // Without it, closing the console kills the process immediately, bypassing graceful shutdown.
        // https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/sigterm-signal-handler
        using var closeHandler = PosixSignalRegistration.Create(PosixSignal.SIGHUP, _ =>
        {
            var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.StopApplication();
            lifetime.ApplicationStopped.WaitHandle.WaitOne();
        });

        await host.RunAsync();
    }
}
