using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Kwerty.DviZe.Logging.Console;

public sealed class DviZeConsole<TTemplate> : ConsoleFormatter, IDisposable where TTemplate : DviZeConsoleTemplate, new()
{
    [ThreadStatic] static TTemplate templateThreadStatic;
    [ThreadStatic] static DviZeConsoleTemplateWritingContext templateWritingContextThreadStatic;
    DviZeConsoleOptions<TTemplate> options;
    readonly IDisposable optionsSubscription;

    public DviZeConsole(string name, IOptionsMonitor<DviZeConsoleOptions<TTemplate>> options)
        : base(name)
    {
        this.options = options.Get(Name);
        optionsSubscription = options.OnChange((opts, name) =>
        {
            if (name == Name)
            {
                this.options = opts;
            }
        });
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        var options = this.options;
        var namespaceResolver = DviZeConsoleNamespaceResolver.GetResolver(options.LoggerCategoriesAreUnbounded);

        if (logEntry.State is BufferedLogRecord bufferedRecord)
        {
            var dviZeLogEntry = new DviZeConsoleLogEntry<object>(scopeProvider: null, namespaceResolver)
            {
                WasBuffered = true,
                Timestamp = options.UseUtcTimestamp ? bufferedRecord.Timestamp : bufferedRecord.Timestamp.ToLocalTime(),
                LogLevel = bufferedRecord.LogLevel,
                Category = logEntry.Category,
                EventId = bufferedRecord.EventId,
                State = bufferedRecord.Attributes,
                BufferedMessage = !string.IsNullOrEmpty(bufferedRecord.FormattedMessage) ? bufferedRecord.FormattedMessage : null,
                BufferedExceptionString = !string.IsNullOrEmpty(bufferedRecord.Exception) ? bufferedRecord.Exception : null,
            };
            WriteCore(in dviZeLogEntry, textWriter, options);
        }
        else
        {
            var dviZeLogEntry = new DviZeConsoleLogEntry<TState>(scopeProvider, namespaceResolver)
            {
                Timestamp = options.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now,
                LogLevel = logEntry.LogLevel,
                Category = logEntry.Category,
                EventId = logEntry.EventId,
                State = logEntry.State,
                Exception = logEntry.Exception,
                MessageFormatter = logEntry.Formatter,
            };
            WriteCore(in dviZeLogEntry, textWriter, options);
        }
    }

    static void WriteCore<TState>(in DviZeConsoleLogEntry<TState> dviZeLogEntry, TextWriter textWriter, DviZeConsoleOptions<TTemplate> options)
    {
        var template = templateThreadStatic ??= new TTemplate();
        template.Reset(options);

        foreach (var composer in options.composers)
        {
            composer.Compose(in dviZeLogEntry, template);
        }

        var writingContext = templateWritingContextThreadStatic ??= new DviZeConsoleTemplateWritingContext();
        writingContext.Reset(textWriter, options);

        template.Write(in dviZeLogEntry, writingContext);

        writingContext.WriteLine();

        if (options.EffectiveColorBehavior == LoggerColorBehavior.Enabled)
        {
            writingContext.Write(DviZeConsoleColor.colorResetCode);
        }
    }

    public void Dispose()
    {
        optionsSubscription.Dispose();
    }
}
