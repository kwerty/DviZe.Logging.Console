using Kwerty.DviZe.Logging.Console.Templates;
using System;

namespace Kwerty.DviZe.Logging.Console.Composers;

public abstract class FilteredComposerBase<TTemplate>
    : DviZeConsoleComposer, IDviZeConsoleComposer<TTemplate> where TTemplate : IDefaultTemplate
{
    protected readonly FilteringCriteria filteringCriteria;

    public FilteredComposerBase(FilteringCriteria filteringCriteria)
    {
        ArgumentNullException.ThrowIfNull(filteringCriteria, nameof(filteringCriteria));
        this.filteringCriteria = filteringCriteria;
    }

    public void Compose<TState>(in DviZeConsoleLogEntry<TState> logEntry, TTemplate template)
    {
        if (filteringCriteria.Category != null)
        {
            if (!logEntry.Category.Equals(filteringCriteria.Category, StringComparison.Ordinal))
            {
                return;
            }
        }

        if (filteringCriteria.Namespace != null)
        {
            if (filteringCriteria.MatchNamespaceExactly)
            {
                if (!logEntry.Namespace.Equals(filteringCriteria.Namespace, StringComparison.Ordinal))
                {
                    return;
                }
            }
            else
            {
                var isMatch = logEntry.Namespace.StartsWith(filteringCriteria.Namespace, StringComparison.Ordinal)
                    && (logEntry.Namespace.Length == filteringCriteria.Namespace.Length
                        || logEntry.Namespace[filteringCriteria.Namespace.Length] == '.');

                if (!isMatch)
                {
                    return;
                }
            }
        }

        if (filteringCriteria.ClassName != null)
        {
            if (logEntry.ClassName == null)
            {
                return;
            }

            if (!logEntry.ClassName.Equals(filteringCriteria.ClassName, StringComparison.Ordinal))
            {
                return;
            }
        }

        ComposeCore(in logEntry, template);
    }

    protected abstract void ComposeCore<TState>(in DviZeConsoleLogEntry<TState> logEntry, TTemplate template);
}
