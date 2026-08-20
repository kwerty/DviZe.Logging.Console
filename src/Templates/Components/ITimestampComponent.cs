using System;

namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public interface ITimestampComponent : IDviZeConsoleTemplate
{
    DateTimeOffset? Value { set; }
}
