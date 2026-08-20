using Kwerty.DviZe.Logging.Console.Templates;
using System;

namespace Kwerty.DviZe.Logging.Console.Composers;

public sealed class ColorComposer<TTemplate>
    : FilteredComposerBase<TTemplate> where TTemplate : IDefaultTemplate
{
    readonly DviZeConsoleColorPair colorPair;

    public ColorComposer(DviZeConsoleColorPair colorPair, FilteringCriteria filteringCriteria)
        : base(filteringCriteria)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(colorPair, default, nameof(colorPair));
        this.colorPair = colorPair;
    }

    protected override void ComposeCore<TState>(in DviZeConsoleLogEntry<TState> logEntry, TTemplate template)
    {
        if (colorPair.ForegroundColor != default)
        {
            template.Painter.ForegroundColor = colorPair.ForegroundColor;
        }

        if (colorPair.BackgroundColor != default)
        {
            template.Painter.BackgroundColor = colorPair.BackgroundColor;
        }
    }
}
