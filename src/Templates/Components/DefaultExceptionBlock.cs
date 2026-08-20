using System;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultExceptionBlock : DviZeConsoleTemplate, IExceptionComponent
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

    string IExceptionComponent.Value
    {
        set
        {
            isOverridden = true;
            overrideValue = value;
        }
    }

    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        var effectiveValue = GetEffectiveValue(in logEntry);
        if (effectiveValue != null)
        {
            writingContext.WriteLine();

            foreach (var line in effectiveValue.AsSpan().EnumerateLines())
            {
                writingContext.Write("│ ");
                writingContext.WriteLine(line);
            }
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
            return logEntry.BufferedExceptionString;
        }
        else
        {
            if (!didCompute)
            {
                var computedValue = logEntry.Exception?.ToString();
                this.computedValue = !string.IsNullOrEmpty(computedValue) ? computedValue : null;
                didCompute = true;
            }
            return computedValue;
        }
    }
}
