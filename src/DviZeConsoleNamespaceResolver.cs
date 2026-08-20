using System;
using System.Collections.Concurrent;
using System.Reflection.Metadata;

namespace Kwerty.DviZe.Logging.Console;

internal class DviZeConsoleNamespaceResolver
{
    static readonly DviZeConsoleNamespaceResolver uncached = new();
    static readonly DviZeConsoleNamespaceResolver cached = new CachedImpl();

    DviZeConsoleNamespaceResolver()
    {
    }

    public virtual (string Namespace, string ClassName) Resolve(string category)
        => ResolveCore(category);

    static (string Namespace, string ClassName) ResolveCore(string category)
    {
        if (!TypeName.TryParse(category.AsSpan(), out var typeName))
        {
            return default;
        }

        var top = typeName;
        while (top.IsNested)
        {
            top = top.DeclaringType;
        }

        if (string.IsNullOrEmpty(top.Namespace))
        {
            return (null, typeName.Name);
        }

        return (top.Namespace, typeName.Name);
    }

    public static DviZeConsoleNamespaceResolver GetResolver(bool loggerCategoriesAreUnbounded)
        => loggerCategoriesAreUnbounded ? uncached : cached;

    sealed class CachedImpl : DviZeConsoleNamespaceResolver
    {
        static readonly ConcurrentDictionary<string, (string Namespace, string ClassName)> cache = [];

        public override (string Namespace, string ClassName) Resolve(string category)
            => cache.GetOrAdd(category, ResolveCore);
    }
}
