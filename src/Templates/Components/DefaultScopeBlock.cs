using System.Collections.Generic;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public sealed class DefaultScopeBlock : DviZeConsoleTemplate, IScopeComponent
{
    readonly List<string> items = [];

    public void AddItem(string item)
    {
        items.Add(item);
    }

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        items.Clear();
    }

    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        writingContext.WriteLine();

        foreach (var item in items)
        {
            WriteItem(item, writingContext);
        }

        if (options.IncludeScopes)
        {
            logEntry.ForEachScope(WriteItem, writingContext);
        }
    }

    static void WriteItem(object item, DviZeConsoleTemplateWritingContext writingContext)
    {
        writingContext.Write("╰──► ");
        writingContext.Write(item.ToString());
        writingContext.WriteLine();
    }
}
