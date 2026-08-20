using Kwerty.DviZe.Logging.Console.Composers;
using Kwerty.DviZe.Logging.Console.Templates;

namespace Kwerty.DviZe.Logging.Console;

public static class DviZeConsoleBuilderExtensions
{
    extension<TTemplate>(DviZeConsoleBuilder<TTemplate> builder) where TTemplate : IDefaultTemplate
    {
        public DviZeConsoleBuilder<TTemplate> StripNamespace(string namesp, bool matchNamespaceExactly = false)
            => builder.AddComposer(new StripNamespaceComposer<TTemplate>(namesp, matchNamespaceExactly));

        public DviZeConsoleBuilder<TTemplate> UseColorsForCategory(string category, DviZeConsoleColorPair colorPair)
            => builder.AddComposer(new ColorComposer<TTemplate>(colorPair, FilteringCriteria.FilterByCategory(category)));

        public DviZeConsoleBuilder<TTemplate> UseColorsForNamespace(string namesp, DviZeConsoleColorPair colorPair)
            => builder.AddComposer(new ColorComposer<TTemplate>(colorPair, FilteringCriteria.FilterByNamespace(namesp)));

        public DviZeConsoleBuilder<TTemplate> UseColorsForNamespace(string namesp, bool matchNamespaceExactly, DviZeConsoleColorPair colorPair)
            => builder.AddComposer(new ColorComposer<TTemplate>(colorPair, FilteringCriteria.FilterByNamespace(namesp, matchNamespaceExactly)));

        public DviZeConsoleBuilder<TTemplate> UseColorsForClass(string className, DviZeConsoleColorPair colorPair)
            => builder.AddComposer(new ColorComposer<TTemplate>(colorPair, FilteringCriteria.FilterByClassName(className)));

        public DviZeConsoleBuilder<TTemplate> UseForegroundColorForCategory(string category, DviZeConsoleColor color)
            => builder.UseColorsForCategory(category, new DviZeConsoleColorPair(color, default));

        public DviZeConsoleBuilder<TTemplate> UseForegroundColorForNamespace(string namesp, DviZeConsoleColor color)
            => builder.UseColorsForNamespace(namesp, new DviZeConsoleColorPair(color, default));

        public DviZeConsoleBuilder<TTemplate> UseForegroundColorForNamespace(string namesp, bool matchNamespaceExactly, DviZeConsoleColor color)
            => builder.UseColorsForNamespace(namesp, matchNamespaceExactly, new DviZeConsoleColorPair(color, default));

        public DviZeConsoleBuilder<TTemplate> UseForegroundColorForClass(string className, DviZeConsoleColor color)
            => builder.UseColorsForClass(className, new DviZeConsoleColorPair(color, default));

        public DviZeConsoleBuilder<TTemplate> UseBackgroundColorForCategory(string category, DviZeConsoleColor color)
            => builder.UseColorsForCategory(category, new DviZeConsoleColorPair(default, color));

        public DviZeConsoleBuilder<TTemplate> UseBackgroundColorForNamespace(string namesp, DviZeConsoleColor color)
            => builder.UseColorsForNamespace(namesp, new DviZeConsoleColorPair(default, color));

        public DviZeConsoleBuilder<TTemplate> UseBackgroundColorForNamespace(string namesp, bool matchNamespaceExactly, DviZeConsoleColor color)
            => builder.UseColorsForNamespace(namesp, matchNamespaceExactly, new DviZeConsoleColorPair(default, color));

        public DviZeConsoleBuilder<TTemplate> UseBackgroundColorForClass(string className, DviZeConsoleColor color)
            => builder.UseColorsForClass(className, new DviZeConsoleColorPair(default, color));
    }
}
