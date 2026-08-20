using Microsoft.Extensions.Logging;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultEventIdField : DviZeConsoleTemplate, IEventIdComponent
{
    bool isOverridden;
    EventId? overrideValue;

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        isOverridden = false;
        overrideValue = null;
    }

    EventId? IEventIdComponent.Value
    {
        set
        {
            isOverridden = true;
            overrideValue = value;
        }
    }

    public bool HasValue<TState>(in DviZeConsoleLogEntry<TState> logEntry)
        => GetEffectiveValue(in logEntry) != null;

    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        var effectiveValue = GetEffectiveValue(in logEntry);
        if (effectiveValue.HasValue)
        {
            writingContext.Write(effectiveValue.Value.Id.ToString());
        }
    }

    EventId? GetEffectiveValue<TState>(in DviZeConsoleLogEntry<TState> logEntry)
        => isOverridden ? overrideValue : logEntry.EventId;
}
