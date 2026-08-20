using Roslyn.Utilities;
using System;

namespace Kwerty.DviZe.Logging.Console.Composers;

public sealed class FilteringCriteria
{
    FilteringCriteria()
    {
    }

    public string Category { get; private init; }

    public string Namespace { get; private init; }

    public bool MatchNamespaceExactly { get; private init; }

    public string ClassName { get; private init; }

    public static FilteringCriteria FilterByCategory(string category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category, nameof(category));
        return new FilteringCriteria { Category = category };
    }

    public static FilteringCriteria FilterByNamespace(string namesp, bool matchNamespaceExactly = false)
    {
        ValidateNamespace(namesp);
        return new FilteringCriteria { Namespace = namesp, MatchNamespaceExactly = matchNamespaceExactly };
    }

    public static FilteringCriteria FilterByClassName(string className)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(className, nameof(className));
        return new FilteringCriteria { ClassName = className };
    }

    public static readonly FilteringCriteria None = new();

    static void ValidateNamespace(string namesp)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(namesp, nameof(namesp));

        foreach (var segment in namesp.Split('.'))
        {
            if (!UnicodeCharacterUtilities.IsValidIdentifier(segment))
            {
                throw new ArgumentOutOfRangeException(nameof(namesp));
            }
        }
    }
}
