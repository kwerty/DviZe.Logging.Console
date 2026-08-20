using System;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultTimestampField : DviZeConsoleTemplate, ITimestampComponent
{
    bool isOverridden;
    DateTimeOffset? overrideValue;

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        isOverridden = false;
        overrideValue = null;
    }

    DateTimeOffset? ITimestampComponent.Value
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
            writingContext.Write(effectiveValue.Value.ToString(options.TimestampFormat));
        }
    }

    DateTimeOffset? GetEffectiveValue<TState>(in DviZeConsoleLogEntry<TState> logEntry)
        => isOverridden ? overrideValue : logEntry.Timestamp;
}
