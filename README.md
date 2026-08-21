# DviZeConsole

A custom `ConsoleFormatter` for the .NET `ConsoleLoggerProvider`, unlocking extensive customization without sacrificing hot-path performance.

📐 Templates let you decide which fields get written to the console, and how they're written.

🧩 Composers execute before the template is written, enabling per-entry customization, and surfacing information normally hidden away in log entry state (`TState`) and structured logging key/value pairs.

✂️ Built-in `StripNamespaceComposer` lets you strip namespaces down to what matters, eg.. `Kwerty.DviZe.Win.Hooks` becomes `Win.Hooks`.

🎨 Built-in `ColorComposer` lets you customize colors by category, namespace, etc..

⚡ Built for the hot path, goes to great lengths to keep allocations to the absolute minimum.

Targets .NET 10. Written in C#.

```csharp
// Installing DviZeConsole with a custom template.
// You can omit TTemplate if you just want to use the built-in DefaultTemplate.
loggingBuilder.AddDviZeConsole<YourCustomTemplate>(consoleBuilder =>
{
    consoleBuilder
        .AddComposer<YourCustomComposer>()
        .StripNamespace("Kwerty.DviZe")
        .UseForegroundColorForNamespace("Microsoft", DviZeConsoleColor.Cyan)
        .ConfigureDefaultColors(new Dictionary<LogLevel, DviZeConsoleColorPair>()
        {
            [LogLevel.Trace] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
            [LogLevel.Debug] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
            [LogLevel.Information] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
            [LogLevel.Warning] = new(DviZeConsoleColor.Black, DviZeConsoleColor.DarkYellow),
            [LogLevel.Error] = new(DviZeConsoleColor.White, DviZeConsoleColor.DarkRed),
            [LogLevel.Critical] = new(DviZeConsoleColor.White, DviZeConsoleColor.DarkRed),
        });
});

// A minimalist template which writes only the timestamp and message to the console.
// Implements IDefaultTemplate for compatibility with built-in composers.
public class YourCustomTemplate : DviZeConsoleTemplate, IDefaultTemplate
{
    readonly DefaultPainter painter = new();
    readonly DefaultTimestampField timestampField = new();
    readonly DefaultMessageField messageField = new();

    // Templates are reused across log entries to keep the hot path allocation free.
    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        painter.Reset(options);
        timestampField.Reset(options);
        messageField.Reset(options);
    }

    IPainter IDefaultTemplate.Painter => painter;

    ITimestampComponent IDefaultTemplate.Timestamp => timestampField;

    IMessageComponent IDefaultTemplate.Message => messageField;

    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        painter.Write(in logEntry, writingContext);
        timestampField.Write(in logEntry, writingContext);
        writingContext.Write(' ');
        messageField.Write(in logEntry, writingContext);
    }
}

// A custom composer which inspects log entry state (TState) for a sensitive-material flag and redacts the message accordingly.
public class YourCustomComposer : DviZeConsoleComposer, IDviZeConsoleComposer<IDefaultTemplate>
{
    public void Compose<TState>(in DviZeConsoleLogEntry<TState> logEntry, IDefaultTemplate template)
    {
        if (logEntry.State is YourCustomLogState customLogState
            && customLogState.ContainsSensitiveMaterial)
        {
            template.Painter.BackgroundColor = DviZeConsoleColor.DarkGray;
            template.Message.Value = "[Redacted]";
        }
    }
}
```

For more, browse the [examples](examples/) directory.
