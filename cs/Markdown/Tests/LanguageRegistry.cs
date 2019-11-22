using System;
using Markdown.Languages;

namespace Markdown.Tests
{
    public static class LanguageRegistry
    {
        public static ILanguage BuildLanguage(string typeName)
        {
            return (ILanguage) Activator.CreateInstance(Type.GetType($"Markdown.Languages.{typeName}") ??
                                                        throw new ArgumentException());
        }
    }
}