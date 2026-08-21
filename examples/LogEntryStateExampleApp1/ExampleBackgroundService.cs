using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogEntryStateExampleApp1;

public class ExampleBackgroundService(ILogger<ExampleBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000);

        logger.LogInformation("Hello {user}, today is {dayName:dddd}.", Environment.UserName, DateTime.Now);

        await Task.Delay(1000);

        var customLogState = new CustomLogState
        {
            SomeValue = "Some value",
            SomeOtherValue = true,
        };
        logger.Log(LogLevel.Information, eventId: 0, customLogState, exception: null, static  (_, _) => "Log message with custom log state.");

        await Task.Delay(1000);

        var sensitiveLogState = new SensitiveLogState
        {
            ContainsSensitiveMaterial = true,
        };
        logger.Log(LogLevel.Information, eventId: 0, sensitiveLogState, exception: null, static (_, _) => "This message contains sensitive material.");

        logger.LogInformation("Done.");
    }
}
