namespace Kwerty.DviZe.Logging.Console;

public readonly record struct DviZeConsoleColorPair(DviZeConsoleColor ForegroundColor, DviZeConsoleColor BackgroundColor)
{
    public static DviZeConsoleColorPair Merge(DviZeConsoleColorPair pair1, DviZeConsoleColorPair pair2)
    {
        var foregroundColor = pair2.ForegroundColor != default ? pair2.ForegroundColor : pair1.ForegroundColor;
        var backgroundColor = pair2.BackgroundColor != default ? pair2.BackgroundColor : pair1.BackgroundColor;
        return new DviZeConsoleColorPair(foregroundColor, backgroundColor);
    }
}
