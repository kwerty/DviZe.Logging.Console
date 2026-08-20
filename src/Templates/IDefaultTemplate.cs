using Kwerty.DviZe.Logging.Console.Templates.Components;
using Microsoft.Extensions.Logging;
using System;

namespace Kwerty.DviZe.Logging.Console.Templates;

public interface IDefaultTemplate : IDviZeConsoleTemplate
{
    private static readonly PainterPlaceholder painterPlaceholder = new();
    private static readonly TimestampPlaceholder timestampPlaceholder = new();
    private static readonly LogLevelPlaceholder logLevelPlaceholder = new();
    private static readonly CategoryPlaceholder categoryPlaceholder = new();
    private static readonly EventIdPlaceholder eventIdPlaceholder = new();
    private static readonly MessagePlaceholder messagePlaceholder = new();
    private static readonly ScopePlaceholder scopePlaceholder = new();
    private static readonly ExceptionPlaceholder exceptionPlaceholder = new();

    IPainter Painter => painterPlaceholder;

    ITimestampComponent Timestamp => timestampPlaceholder;

    ILogLevelComponent LogLevel => logLevelPlaceholder;

    ICategoryComponent Category => categoryPlaceholder;

    IEventIdComponent EventId => eventIdPlaceholder;

    IMessageComponent Message => messagePlaceholder;

    IScopeComponent Scope => scopePlaceholder;

    IExceptionComponent Exception => exceptionPlaceholder;

    sealed class PainterPlaceholder : IPainter
    {
        public DviZeConsoleColorPair Colors { set => _ = value; }

        public DviZeConsoleColor ForegroundColor { set => _ = value; }

        public DviZeConsoleColor BackgroundColor { set => _ = value; }
    }

    sealed class TimestampPlaceholder : ITimestampComponent
    {
        public DateTimeOffset? Value { set => _ = value; }
    }

    sealed class LogLevelPlaceholder : ILogLevelComponent;

    sealed class CategoryPlaceholder : ICategoryComponent
    {
        public string Value { set => _ = value; }
    }

    sealed class EventIdPlaceholder : IEventIdComponent
    {
        public EventId? Value { set => _ = value; }
    }

    sealed class MessagePlaceholder : IMessageComponent
    {
        public string Value { set => _ = value; }
    }

    sealed class ScopePlaceholder : IScopeComponent
    {
        public void AddItem(string item) { }
    }

    sealed class ExceptionPlaceholder : IExceptionComponent
    {
        public string Value { set => _ = value; }
    }
}
