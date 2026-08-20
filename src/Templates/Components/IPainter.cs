namespace Kwerty.DviZe.Logging.Console.Templates.Components;

public interface IPainter : IDviZeConsoleTemplate
{
    DviZeConsoleColorPair Colors { set; }

    DviZeConsoleColor ForegroundColor { set; }

    DviZeConsoleColor BackgroundColor { set; }
}
