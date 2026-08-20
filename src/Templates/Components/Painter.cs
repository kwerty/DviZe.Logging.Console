using Microsoft.Extensions.Logging.Console;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public class Painter(DviZeConsoleColorPair initial = default) : DviZeConsoleTemplate, IPainter // Todo: Use 'closed' modifier when .NET 11 is released.
{
    protected DviZeConsoleColorPair colorPair;

    public Painter(DviZeConsoleColor foregroundColor, DviZeConsoleColor backgroundColor = default)
        : this(new DviZeConsoleColorPair(foregroundColor, backgroundColor))
    {
    }

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        colorPair = initial;
    }

    DviZeConsoleColorPair IPainter.Colors
    {
        set => colorPair = value;
    }

    DviZeConsoleColor IPainter.ForegroundColor
    {
        set => colorPair = colorPair with { ForegroundColor = value };
    }

    DviZeConsoleColor IPainter.BackgroundColor
    {
        set => colorPair = colorPair with { BackgroundColor = value };
    }

    public sealed override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        if (options.EffectiveColorBehavior == LoggerColorBehavior.Enabled)
        {
            WriteCore(logEntry, writingContext);
        }
    }

    protected virtual void WriteCore<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        if (colorPair.ForegroundColor != default)
        {
            writingContext.WriteAnsiCodes(colorPair.ForegroundColor.foregroundCode);
        }

        if (colorPair.BackgroundColor != default)
        {
            writingContext.WriteAnsiCodes(colorPair.BackgroundColor.backgroundCode);
        }
    }
}
