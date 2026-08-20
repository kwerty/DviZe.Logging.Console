namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultMessageField : DviZeConsoleTemplate, IMessageComponent
{
    bool didCompute;
    string computedValue;
    bool isOverridden;
    string overrideValue;

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        didCompute = false;
        computedValue = null;
        isOverridden = false;
        overrideValue = null;
    }

    string IMessageComponent.Value
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
        if (effectiveValue != null)
        {
            writingContext.Write(effectiveValue);
        }
    }

    string GetEffectiveValue<TState>(in DviZeConsoleLogEntry<TState> logEntry)
    {
        if (isOverridden)
        {
            return overrideValue;
        }
        else if (logEntry.WasBuffered)
        {
            return logEntry.BufferedMessage;
        }
        else
        {
            if (!didCompute)
            {
                var computedValue = logEntry.MessageFormatter(logEntry.State, logEntry.Exception);
                this.computedValue = !string.IsNullOrEmpty(computedValue) ? computedValue : null;
            }
            return computedValue;
        }
    }
}
