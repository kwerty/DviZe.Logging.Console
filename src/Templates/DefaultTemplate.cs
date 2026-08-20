using Kwerty.DviZe.Logging.Console.Templates.Components;

namespace Kwerty.DviZe.Logging.Console.Templates;

public sealed class DefaultTemplate : DviZeConsoleTemplate, IDefaultTemplate
{
    readonly DefaultPainter painter = new();
    readonly DefaultTimestampField timestampField = new();
    readonly DefaultLogLevelField logLevelField = new();
    readonly DefaultCategoryField categoryField = new();
    readonly DefaultEventIdField eventIdField = new();
    readonly DefaultMessageField messageField = new();
    readonly DefaultScopeBlock scopeBlock = new();
    readonly DefaultExceptionBlock exceptionBlock = new();

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        painter.Reset(options);
        timestampField.Reset(options);
        logLevelField.Reset(options);
        categoryField.Reset(options);
        eventIdField.Reset(options);
        messageField.Reset(options);
        scopeBlock.Reset(options);
        exceptionBlock.Reset(options);
    }

    IPainter IDefaultTemplate.Painter => painter;

    ITimestampComponent IDefaultTemplate.Timestamp => timestampField;

    ILogLevelComponent IDefaultTemplate.LogLevel => logLevelField;

    ICategoryComponent IDefaultTemplate.Category => categoryField;

    IEventIdComponent IDefaultTemplate.EventId => eventIdField;

    IMessageComponent IDefaultTemplate.Message => messageField;

    IScopeComponent IDefaultTemplate.Scope => scopeBlock;

    IExceptionComponent IDefaultTemplate.Exception => exceptionBlock;

    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        painter.Write(in logEntry, writingContext);

        if (timestampField.HasValue(in logEntry))
        {
            timestampField.Write(in logEntry, writingContext);
            writingContext.Write(' ');
        }

        logLevelField.Write(in logEntry, writingContext);
        writingContext.Write(' ');

        if (categoryField.HasValue(in logEntry))
        {
            categoryField.Write(in logEntry, writingContext);
        }

        if (eventIdField.HasValue(in logEntry))
        {
            writingContext.Write('[');
            eventIdField.Write(in logEntry, writingContext);
            writingContext.Write(']');
        }

        if (messageField.HasValue(in logEntry))
        {
            writingContext.Write(' ');
            messageField.Write(in logEntry, writingContext);
        }

        scopeBlock.Write(in logEntry, writingContext);

        exceptionBlock.Write(in logEntry, writingContext);
    }
}
