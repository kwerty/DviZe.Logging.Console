using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kwerty.DviZe.Logging.Console;

public abstract class DviZeConsoleOptions // Todo: Use 'closed' modifier when .NET 11 is released.
{
    public bool LoggerCategoriesAreUnbounded { get; set; }

    public bool IncludeScopes { get; set; } = true;

    public string TimestampFormat { get; set; } = "HH:mm:ss";

    public bool UseUtcTimestamp { get; set; }

    public LoggerColorBehavior ColorBehavior { get; set; }

    public LoggerColorBehavior EffectiveColorBehavior { get; internal set; }

    public Dictionary<LogLevel, DviZeConsoleColorPair> DefaultColors { get; set; } = new()
    {
        [LogLevel.Trace] = new(DviZeConsoleColor.Magenta, DviZeConsoleColor.Default),
        [LogLevel.Debug] = new(DviZeConsoleColor.Magenta, DviZeConsoleColor.Default),
        [LogLevel.Information] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
        [LogLevel.Warning] = new(DviZeConsoleColor.Yellow, DviZeConsoleColor.Default),
        [LogLevel.Error] = new(DviZeConsoleColor.Red, DviZeConsoleColor.Default),
        [LogLevel.Critical] = new(DviZeConsoleColor.Red, DviZeConsoleColor.Default),
    };
}

public sealed class DviZeConsoleOptions<TTemplate>
    : DviZeConsoleOptions, IPostConfigureOptions<DviZeConsoleOptions<TTemplate>>, IValidateOptions<DviZeConsoleOptions<TTemplate>> where TTemplate : IDviZeConsoleTemplate
{
    internal IDviZeConsoleComposer<TTemplate>[] composers = [];

    void IPostConfigureOptions<DviZeConsoleOptions<TTemplate>>.PostConfigure(string name, DviZeConsoleOptions<TTemplate> options)
    {
        options.EffectiveColorBehavior = options.ColorBehavior != LoggerColorBehavior.Default
            ? options.ColorBehavior
            : ConsoleUtils.EmitAnsiColorCodes ? LoggerColorBehavior.Enabled : LoggerColorBehavior.Disabled;
    }

    ValidateOptionsResult IValidateOptions<DviZeConsoleOptions<TTemplate>>.Validate(string name, DviZeConsoleOptions<TTemplate> options)
    {
        var errors = new List<string>();

        if (options.DefaultColors == null
            || options.DefaultColors.Count != 6)
        {
            errors.Add($"{nameof(DviZeConsoleOptions)}.{nameof(DefaultColors)} must not be null, and must contain entries for all six log levels.");
        }
        else
        {
            if (options.DefaultColors.Keys.Any(logLevel => logLevel < LogLevel.Trace || logLevel >= LogLevel.None))
            {
                errors.Add($"{nameof(DviZeConsoleOptions)}.{nameof(DefaultColors)} contains invalid entries.");
            }
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }
}
