using Kwerty.DviZe.Logging.Console;
using Kwerty.DviZe.Logging.Console.Templates;
using Kwerty.DviZe.Logging.Console.Templates.Components;

namespace CustomTemplateExampleApp1;

public class CustomTemplate : DviZeConsoleTemplate, IDefaultTemplate // Implement IDefaultTemplate for compatibility with built-in composers.
{
    readonly DefaultPainter painter = new();
    readonly DefaultTimestampField timestampField = new();
    readonly DefaultLogLevelField logLevelField = new();
    readonly PaintRemover paintRemover = new();
    readonly DefaultMessageField messageField = new();
    readonly DefaultScopeBlock scopeBlock = new();
    readonly DefaultExceptionBlock exceptionBlock = new();

    public override void Reset(DviZeConsoleOptions options)
    {
        base.Reset(options);
        painter.Reset(options);
        timestampField.Reset(options);
        logLevelField.Reset(options);
        paintRemover.Reset(options);
        messageField.Reset(options);
        scopeBlock.Reset(options);
        exceptionBlock.Reset(options);
    }

    IPainter IDefaultTemplate.Painter => painter;

    ITimestampComponent IDefaultTemplate.Timestamp => timestampField;

    IMessageComponent IDefaultTemplate.Message => messageField;

    IScopeComponent IDefaultTemplate.Scope => scopeBlock;

    IExceptionComponent IDefaultTemplate.Exception => exceptionBlock;

    public override void Write<TState>(in DviZeConsoleLogEntry<TState> logEntry, DviZeConsoleTemplateWritingContext writingContext)
    {
        if (timestampField.HasValue(in logEntry))
        {
            timestampField.Write(in logEntry, writingContext);
            writingContext.Write(" ☻ ");
        }

        painter.Write(in logEntry, writingContext);

        logLevelField.Write(in logEntry, writingContext);

        paintRemover.Write(in logEntry, writingContext);

        if (messageField.HasValue(in logEntry))
        {
            writingContext.Write(' ');
            messageField.Write(in logEntry, writingContext);
        }

        scopeBlock.Write(in logEntry, writingContext);

        exceptionBlock.Write(in logEntry, writingContext);
    }
}
