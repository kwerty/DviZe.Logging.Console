using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomComposerExampleApp1;

public class ExampleBackgroundService(ILogger<ExampleBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000);

        logger.LogDebug("Hello {user}, today is {dayName:dddd}.", Environment.UserName, DateTime.Now);

        await Task.Delay(1000);

        logger.LogInformation("Done.");
    }
}
