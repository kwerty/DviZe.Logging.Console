using Microsoft.Extensions.Logging;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultLogLevelField : DviZeConsoleTemplate, ILogLevelComponent
{
    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        writingContext.Write(logEntry.LogLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => string.Empty,
        });
    }
}
