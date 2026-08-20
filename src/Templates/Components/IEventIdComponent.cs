using Microsoft.Extensions.Logging;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public interface IEventIdComponent : IDviZeConsoleTemplate
{
    EventId? Value { set; }
}
