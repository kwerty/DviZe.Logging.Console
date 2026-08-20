namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultCategoryField : DviZeConsoleTemplate, ICategoryComponent
{
    bool isOverridden;
    string overrideValue;

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        isOverridden = false;
        overrideValue = null;
    }

    string ICategoryComponent.Value
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
        => isOverridden ? overrideValue : logEntry.Category;
}
