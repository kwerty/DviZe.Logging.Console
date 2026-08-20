using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;

namespace Kwerty.DviZe.Logging.Console;

public sealed class DviZeConsoleBuilder<TTemplate> where TTemplate : IDviZeConsoleTemplate
{
    readonly DviZeConsoleOptions<TTemplate> options;

    internal DviZeConsoleBuilder(DviZeConsoleOptions<TTemplate> options)
    {
        this.options = options;
    }

    /// <summary>
    /// Parsing a category into its namespace and class name parts isn't free, so the result is cached by default.
    /// This can become a problem when loggers use dynamically generated categories (eg.. "User1", "User2", "User3"),
    /// since the set of distinct categories is effectively unbounded, causing each one to leak memory for the life
    /// of the process. In practice this is rarely an issue, since convention is to use the fully qualified class name
    /// as the category (dynamic categories are generally considered an anti-pattern), but if you're stuck with
    /// misbehaving loggers you should pass <see langword="true"/> to disable caching, at the cost of re-parsing the
    /// category every time.
    /// </summary>
    public DviZeConsoleBuilder<TTemplate> LoggerCategoriesAreUnbounded(bool loggerCategoriesAreUnbounded)
    {
        options.LoggerCategoriesAreUnbounded = loggerCategoriesAreUnbounded;
        return this;
    }

    public DviZeConsoleBuilder<TTemplate> UseTimestampFormat(string timestampFormat)
    {
        ValidateTimestampFormat(timestampFormat);
        options.TimestampFormat = timestampFormat;
        return this;
    }

    public DviZeConsoleBuilder<TTemplate> UseUtcTimestamp(bool useUtcTimestamp)
    {
        options.UseUtcTimestamp = useUtcTimestamp;
        return this;
    }

    public DviZeConsoleBuilder<TTemplate> IncludeScopes(bool includeScopes)
    {
        options.IncludeScopes = includeScopes;
        return this;
    }

    public DviZeConsoleBuilder<TTemplate> UseColorBehavior(LoggerColorBehavior colorBehavior)
    {
        options.ColorBehavior = colorBehavior;
        return this;
    }

    public DviZeConsoleBuilder<TTemplate> ConfigureDefaultColors(IReadOnlyDictionary<LogLevel, DviZeConsoleColorPair> colorMap)
    {
        ArgumentNullException.ThrowIfNull(colorMap, nameof(colorMap));

        if (colorMap.Count == 0)
        {
            return this;
        }

        var merged = new Dictionary<LogLevel, DviZeConsoleColorPair>(options.DefaultColors);
        foreach (var (logLevel, colorPair) in colorMap)
        {
            merged[logLevel] = DviZeConsoleColorPair.Merge(merged[logLevel], colorPair);
        }

        options.DefaultColors = merged;
        return this;
    }

    public DviZeConsoleBuilder<TTemplate> ConfigureDefaultColors(LogLevel logLevel, DviZeConsoleColorPair colorPair)
    {
        return ConfigureDefaultColors(new Dictionary<LogLevel, DviZeConsoleColorPair>
        {
            [logLevel] = colorPair,
        });
    }

    public DviZeConsoleBuilder<TTemplate> ConfigureDefaultForegroundColor(LogLevel logLevel, DviZeConsoleColor color)
        => ConfigureDefaultColors(logLevel, new DviZeConsoleColorPair(color, default));

    public DviZeConsoleBuilder<TTemplate> ConfigureDefaultBackgroundColor(LogLevel logLevel, DviZeConsoleColor color)
        => ConfigureDefaultColors(logLevel, new DviZeConsoleColorPair(default, color));

    public DviZeConsoleBuilder<TTemplate> AddComposer<TComposer>() where TComposer : DviZeConsoleComposer, IDviZeConsoleComposer<TTemplate>, new()
        => AddComposer(new TComposer());

    public DviZeConsoleBuilder<TTemplate> AddComposer<TComposer>(TComposer composer) where TComposer : DviZeConsoleComposer, IDviZeConsoleComposer<TTemplate>
    {
        ArgumentNullException.ThrowIfNull(composer, nameof(composer));
        composer.options = options;
        options.composers = [.. options.composers, composer];
        return this;
    }

    static void ValidateTimestampFormat(string timestampFormat)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(timestampFormat, nameof(timestampFormat));
        try
        {
            _ = DateTime.UtcNow.ToString(timestampFormat);
        }
        catch (FormatException)
        {
            throw new ArgumentOutOfRangeException(nameof(timestampFormat));
        }
    }
}
