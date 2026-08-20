# DviZeConsole

A `ConsoleFormatter` for the .NET `ConsoleLoggerProvider`, unlocking deep customization without sacrificing hot-path performance.

📐 Templates let you decide which fields get written to the console, and how they're written.

🧩 Composers execute before the template is written, enabling per-entry customization, and surfacing information normally hidden away in log entry state (`TState`) or structured logging key/value pairs.

✂️ Built-in `StripNamespaceComposer` lets you strip namespaces down to what matters, eg.. `Kwerty.DviZe.Win.Hooks` becomes `Win.Hooks`.

🎨 Built-in `ColorComposer` lets you customize colors by category, namespace, etc..

⚡ Built for the hot path, goes to great lengths to keep allocations to the absolute minimum.

Targets .NET 10. Written in C#.

```csharp
builder.Logging
    .AddDviZeConsole(consoleBuilder =>
    {
        consoleBuilder
            .StripNamespace("Kwerty.DviZe")
            .ConfigureDefaultColors(new Dictionary<LogLevel, DviZeConsoleColorPair>()
            {
                [LogLevel.Trace] = new(DviZeConsoleColor.DarkGray, DviZeConsoleColor.Default),
                [LogLevel.Debug] = new(DviZeConsoleColor.DarkGray, DviZeConsoleColor.Default),
                [LogLevel.Information] = new(DviZeConsoleColor.White, DviZeConsoleColor.Default),
                [LogLevel.Warning] = new(DviZeConsoleColor.DarkYellow, DviZeConsoleColor.Default),
                [LogLevel.Error] = new(DviZeConsoleColor.Red, DviZeConsoleColor.Default),
                [LogLevel.Critical] = new(DviZeConsoleColor.Red, DviZeConsoleColor.Default),
            })
            .UseForegroundColorForNamespace("ExampleApp1", DviZeConsoleColor.Magenta);
    });
```

See [Examples](examples/) for more.
