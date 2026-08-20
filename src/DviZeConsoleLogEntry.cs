using Microsoft.Extensions.Logging;
using System;

namespace Kwerty.DviZe.Logging.Console;

public ref struct DviZeConsoleLogEntry<TState>
{
    readonly IExternalScopeProvider scopeProvider;
    readonly DviZeConsoleNamespaceResolver namespaceResolver;
    (string Namespace, string ClassName)? namespaceResult;

    internal DviZeConsoleLogEntry(IExternalScopeProvider scopeProvider, DviZeConsoleNamespaceResolver namespaceResolver)
    {
        this.scopeProvider = scopeProvider;
        this.namespaceResolver = namespaceResolver;
    }

    public readonly bool WasBuffered { get; init; }

    public required readonly DateTimeOffset Timestamp { get; init; }

    public required readonly LogLevel LogLevel { get; init; }

    public required readonly string Category { get; init; }

    public required readonly EventId EventId { get; init; }

    public required readonly TState State { get; init; }

    public readonly Exception Exception { get; init; }

    public readonly string BufferedExceptionString { get; init; }

    public readonly Func<TState, Exception, string> MessageFormatter { get; init; }

    public readonly string BufferedMessage { get; init; }

    public readonly void ForEachScope<TScopeState>(Action<object, TScopeState> callback, TScopeState state)
        => scopeProvider?.ForEachScope(callback, state);

    public string Namespace => (namespaceResult ??= namespaceResolver.Resolve(Category)).Namespace;

    public string ClassName => (namespaceResult ??= namespaceResolver.Resolve(Category)).ClassName;
}
