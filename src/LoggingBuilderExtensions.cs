using Kwerty.DviZe.Logging.Console.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;

namespace Kwerty.DviZe.Logging.Console;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddDviZeConsole(this ILoggingBuilder loggingBuilder, Action<DviZeConsoleBuilder<DefaultTemplate>> configure = null)
        => AddDviZeConsole(loggingBuilder, name: null, configure);

    public static ILoggingBuilder AddDviZeConsole<TTemplate>(this ILoggingBuilder loggingBuilder, Action<DviZeConsoleBuilder<TTemplate>> configure = null) where TTemplate : DviZeConsoleTemplate, new()
        => AddDviZeConsole(loggingBuilder, name: null, configure);

    public static ILoggingBuilder AddDviZeConsole<TTemplate>(this ILoggingBuilder loggingBuilder, string name = null, Action<DviZeConsoleBuilder<TTemplate>> configure = null)
        where TTemplate : DviZeConsoleTemplate, new()
    {
        name ??= nameof(DviZeConsole<>);

        loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<DviZeConsoleOptions<TTemplate>>, DviZeConsoleOptions<TTemplate>>());
        loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<DviZeConsoleOptions<TTemplate>>, DviZeConsoleOptions<TTemplate>>());

        loggingBuilder.Services.AddOptions<DviZeConsoleOptions<TTemplate>>(name)
            .BindConfiguration("Logging:Console:FormatterOptions")
            .BindConfiguration($"Logging:{nameof(DviZeConsole<>)}")
            .BindConfiguration($"Logging:{nameof(DviZeConsole<>)}:{name}");

        if (configure != null)
        {
            loggingBuilder.Services.Configure<DviZeConsoleOptions<TTemplate>>(name, opts => configure(new DviZeConsoleBuilder<TTemplate>(opts)));
        }

        loggingBuilder.Services.AddSingleton<ConsoleFormatter>(svc =>
            new DviZeConsole<TTemplate>(name, svc.GetRequiredService<IOptionsMonitor<DviZeConsoleOptions<TTemplate>>>()));

        return loggingBuilder.AddConsole(options =>
        {
            options.FormatterName ??= name; // If the formatter was set by appsettings.json then we won't override it.
        });
    }
}
