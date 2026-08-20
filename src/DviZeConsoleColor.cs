using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kwerty.DviZe.Logging.Console;

[TypeConverter(typeof(Converter))]
public readonly record struct DviZeConsoleColor
{
    internal const string colorResetCode = "\x1B[0m"; // SGR Reset.
    internal const string backgroundFillCode = "\x1b[K"; // CSI Erase in Line.
    static readonly Dictionary<string, DviZeConsoleColor> colors;
    readonly string name;
    internal readonly string foregroundCode;
    internal readonly string backgroundCode;

    static DviZeConsoleColor()
    {
        colors = new List<DviZeConsoleColor>
        {
            Default,
            Black, DarkRed, DarkGreen, DarkYellow, DarkBlue, DarkMagenta, DarkCyan, Gray,
            DarkGray, Red, Green, Yellow, Blue, Magenta, Cyan, White,
        }.ToDictionary(c => c.ToString(), c => c);
    }

    DviZeConsoleColor(string foregroundCode, string backgroundCode, [CallerMemberName] string name = null)
    {
        this.foregroundCode = foregroundCode;
        this.backgroundCode = backgroundCode;
        this.name = name;
    }

    public override string ToString() => name ?? base.ToString();

    public static DviZeConsoleColor Parse(string str)
    {
        if (!TryParse(str, out var color))
        {
            throw new FormatException($"'{str}' is not a valid {nameof(DviZeConsoleColor)}.");
        }

        return color;
    }

    public static bool TryParse(string str, out DviZeConsoleColor color)
    {
        if (str != null
            && colors.TryGetValue(str, out color))
        {
            return true;
        }

        color = default;
        return false;
    }

    public static readonly DviZeConsoleColor Default = new("\e[39m\e[22m", "\e[49m");

    public static readonly DviZeConsoleColor Black = new("\e[30m", "\e[40m");

    public static readonly DviZeConsoleColor DarkRed = new("\e[31m", "\e[41m");

    public static readonly DviZeConsoleColor DarkGreen = new("\e[32m", "\e[42m");

    public static readonly DviZeConsoleColor DarkYellow = new("\e[33m", "\e[43m");

    public static readonly DviZeConsoleColor DarkBlue = new("\e[34m", "\e[44m");

    public static readonly DviZeConsoleColor DarkMagenta = new("\e[35m", "\e[45m");

    public static readonly DviZeConsoleColor DarkCyan = new("\e[36m", "\e[46m");

    public static readonly DviZeConsoleColor Gray = new("\e[37m", "\e[47m");

    public static readonly DviZeConsoleColor DarkGray = new("\e[90m", "\e[100m");

    public static readonly DviZeConsoleColor Red = new("\e[1m\e[31m", "\e[101m");

    public static readonly DviZeConsoleColor Green = new("\e[1m\e[32m", "\e[102m");

    public static readonly DviZeConsoleColor Yellow = new("\e[1m\e[33m", "\e[103m");

    public static readonly DviZeConsoleColor Blue = new("\e[1m\e[34m", "\e[104m");

    public static readonly DviZeConsoleColor Magenta = new("\e[1m\e[35m", "\e[105m");

    public static readonly DviZeConsoleColor Cyan = new("\e[1m\e[36m", "\e[106m");

    public static readonly DviZeConsoleColor White = new("\e[1m\e[37m", "\e[107m");

    sealed class Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str
                && TryParse(str, out var color))
            {
                return color;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
