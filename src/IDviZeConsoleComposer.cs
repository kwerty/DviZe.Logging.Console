namespace Kwerty.DviZe.Logging.Console;

public interface IDviZeConsoleComposer<in TTemplate> where TTemplate : IDviZeConsoleTemplate
{
    void Compose<TState>(in DviZeConsoleLogEntry<TState> logEntry, TTemplate template);
}
