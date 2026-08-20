using Microsoft.Extensions.Logging.Console;
using System;
using System.IO;

namespace Kwerty.DviZe.Logging.Console;

public sealed class DviZeConsoleTemplateWritingContext
{
    TextWriter textWriter;
    DviZeConsoleOptions options;
    int pos;

    internal DviZeConsoleTemplateWritingContext()
    {
    }

    public void Reset(TextWriter textWriter, DviZeConsoleOptions options)
    {
        this.textWriter = textWriter;
        this.options = options;
        pos = 0;
    }

    public void Write(ReadOnlySpan<char> buffer)
    {
        textWriter.Write(buffer);
        pos += buffer.Length;
    }

    public void Write(char value)
    {
        textWriter.Write(value);
        pos += 1;
    }

    public void WriteLine()
    {
        if (pos > 0)
        {
            var width = System.Console.WindowWidth;

            if (pos < width)
            {
                // Fill background color to the end of the line.
                if (options.EffectiveColorBehavior == LoggerColorBehavior.Enabled)
                {
                    textWriter.Write(DviZeConsoleColor.backgroundFillCode);
                }
            }
            else if (pos == width)
            {
                // Workaround for ANSI wrap ambiguity. Causes an additional empty line to
                // be printed if the line width matches the window width exactly.
                textWriter.Write('░');
            }

            textWriter.WriteLine();
            pos = 0;
        }
    }

    public void WriteLine(ReadOnlySpan<char> buffer)
    {
        textWriter.Write(buffer);
        pos += buffer.Length;
        WriteLine();
    }

    public void WriteAnsiCodes(ReadOnlySpan<char> buffer)
    {
        textWriter.Write(buffer);
    }
}
