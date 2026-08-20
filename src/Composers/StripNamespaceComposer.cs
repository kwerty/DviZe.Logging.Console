using Kwerty.DviZe.Logging.Console.Templates;
using System;
using System.Collections.Concurrent;

namespace Kwerty.DviZe.Logging.Console.Composers;

public sealed class StripNamespaceComposer<TTemplate>(string namesp, bool matchNamespaceExactly)
    : FilteredComposerBase<TTemplate>(FilteringCriteria.FilterByNamespace(namesp, matchNamespaceExactly)) where TTemplate : IDefaultTemplate
{
    static readonly ConcurrentDictionary<(string Namespace, string ClassName, string Prefix), string> cache = [];

    protected override void ComposeCore<TState>(in DviZeConsoleLogEntry<TState> logEntry, TTemplate template)
    {
        if (options.LoggerCategoriesAreUnbounded)
        {
            template.Category.Value = StripNamespace(logEntry.Namespace, logEntry.ClassName, prefix: filteringCriteria.Namespace);
        }
        else
        {
            var key = (logEntry.Namespace, logEntry.ClassName, Prefix: filteringCriteria.Namespace);
            template.Category.Value = cache.GetOrAdd(key, static key => StripNamespace(key.Namespace, key.ClassName, key.Prefix));
        }

        static string StripNamespace(string namesp, string className, string prefix)
        {
            var remaining = namesp.AsSpan(prefix.Length);
            if (remaining.StartsWith('.'))
            {
                remaining = remaining[1..];
            }
            return remaining.Length > 0 ? $"{remaining}.{className}" : className;
        }
    }
}
