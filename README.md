# DviZeConsole

A drop-in .NET console formatter unlocking deep customization and extensibility, without sacrificing hot-path performance.

📐 Templates let you choose which fields get written to the console, and how each one is rendered.

🧩 Composers run before each log entry is rendered, letting you inspect and customize log entries individually.

🔍 Implement your own composer to inspect structured key/value data (or custom `TState`) and use it to customize the entry or render extra detail via the built-in `DefaultScopeBlock`.

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
