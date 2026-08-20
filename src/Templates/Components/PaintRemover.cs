using Microsoft.Extensions.Logging.Console;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class PaintRemover : DviZeConsoleTemplate
{
    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        if (options.EffectiveColorBehavior == LoggerColorBehavior.Enabled)
        {
            writingContext.WriteAnsiCodes(DviZeConsoleColor.colorResetCode);
        }
    }
}
