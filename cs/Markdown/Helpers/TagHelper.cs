using System.Reflection;
using Markdown.Tags;

namespace Markdown.Helpers;

public static class TagHelper
{
    public static IEnumerable<T?>? GetAllTags<T>() =>
        Assembly
            .GetAssembly(typeof(T))?
            .GetTypes()
            .Where(type => type.GetInterfaces().Contains(typeof(T)))
            .Select(item => (T) Activator.CreateInstance(item)!);

    public static (string start, string end) GetHtmlFormat(string tagName) => ($"<{tagName}>", $"</{tagName}>");
}