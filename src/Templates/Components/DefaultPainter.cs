namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultPainter : Painter
{
    protected override void WriteCore<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        if (colorPair.ForegroundColor == default
            || colorPair.BackgroundColor == default)
        {
            colorPair = DviZeConsoleColorPair.Merge(options.DefaultColors[logEntry.LogLevel], colorPair);
        }

        base.WriteCore(logEntry, writingContext);
    }
}
