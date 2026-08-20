namespace Kwerty.DviZe.Logging.Console;

public abstract class DviZeConsoleTemplate : IDviZeConsoleTemplate
{
    protected DviZeConsoleOptions options;

    public virtual void Reset(DviZeConsoleOptions options)
    {
        this.options = options;
    }

    public abstract void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext);
}
