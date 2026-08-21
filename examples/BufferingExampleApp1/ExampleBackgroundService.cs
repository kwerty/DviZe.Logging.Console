using Microsoft.Extensions.Diagnostics.Buffering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BufferingExampleApp1;

public class ExampleBackgroundService(ILogger<ExampleBackgroundService> logger, GlobalLogBuffer logBuffer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Buffered entries don't retain the original TState/structured data, only the formatted message.
        logger.LogInformation("Hello {user}, today is {dayName:dddd}.", Environment.UserName, DateTime.Now);

        logger.LogWarning("This is a warning message.");

        // Buffered entries don't retain the exception instance, but they do retain its ToString() value.
        logger.LogError(new TimeoutException(), "This is an error message with an exception.");

        // Buffered entries don't retain scope information.
        using (logger.BeginScope("Example scope 1."))
        {
            using (logger.BeginScope("Example scope 2."))
            {
                logger.LogInformation("This is message with scope info.");
            }
        }

        logger.LogInformation("Done.");

        logBuffer.Flush(); // Flush all buffered log entries.
    }
}
