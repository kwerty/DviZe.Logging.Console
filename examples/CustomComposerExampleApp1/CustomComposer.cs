using Kwerty.DviZe.Logging.Console;
using Kwerty.DviZe.Logging.Console.Templates;
using Microsoft.Extensions.Logging;
using System;

namespace CustomComposerExampleApp1;

public class CustomComposer : DviZeConsoleComposer, IDviZeConsoleComposer<IDefaultTemplate>
{
    public void Compose<TState>(in DviZeConsoleLogEntry<TState> logEntry, IDefaultTemplate template)
    {
        if (logEntry.LogLevel == LogLevel.Debug
            && logEntry.ClassName == "ExampleBackgroundService")
        {
            template.Timestamp.Value = DateTimeOffset.MinValue;

            template.Category.Value = "OverriddenCategory";

            template.EventId.Value = 67;

            template.Message.Value = "OverriddenMessage";

            template.Scope.AddItem("Custom scope 1");
            template.Scope.AddItem("Custom scope 2");
        }
    }
}
