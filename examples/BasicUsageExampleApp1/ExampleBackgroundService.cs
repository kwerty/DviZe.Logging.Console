using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BasicUsageExampleApp1;

public class ExampleBackgroundService(ILogger<ExampleBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000);

        logger.LogInformation("Hello {user}, today is {dayName:dddd}.", Environment.UserName, DateTime.Now);

        await Task.Delay(1000);

        logger.LogWarning("This is a warning message.");

        await Task.Delay(1000);

        logger.LogError(new TimeoutException(), "This is an error message with an exception.");

        await Task.Delay(1000);

        using (logger.BeginScope("Example scope 1."))
        {
            using (logger.BeginScope("Example scope 2."))
            {
                logger.LogInformation("This is message with scope info.");
            }
        }

        logger.LogInformation("Done.");
    }
}
