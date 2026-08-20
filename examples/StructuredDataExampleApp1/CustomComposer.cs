using Kwerty.DviZe.Logging.Console;
using Kwerty.DviZe.Logging.Console.Templates;
using System.Collections.Generic;

namespace StructuredDataExampleApp1;

public class CustomComposer : DviZeConsoleComposer, IDviZeConsoleComposer<IDefaultTemplate>
{
    public void Compose<TState>(in DviZeConsoleLogEntry<TState> logEntry, IDefaultTemplate template)
    {
        // Extracting structured data and displaying it in the scope block.

        if (logEntry.State is IReadOnlyList<KeyValuePair<string, object>> keyValuePairs)
        {
            foreach (var item in keyValuePairs)
            {
                if (item.Key != "{OriginalFormat}")
                {
                    template.Scope.AddItem($"{item.Key}: {item.Value}");
                }
            }
        }

        // Extracting data from a custom TState and displaying it in the scope block.

        if (logEntry.State is CustomLogState customLogState)
        {
            template.Scope.AddItem($"Some value: {customLogState.SomeValue}");
            template.Scope.AddItem($"Some other value: {customLogState.SomeOtherValue}");
        }
    }
}
