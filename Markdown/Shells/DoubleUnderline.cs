using System;
using System.Collections.Generic;

namespace Markdown.Shells
{
    public class DoubleUnderline : IShell
    {
        private const string Prefix = "__";
        private const string Suffix = "__";
        private readonly List<Type> innerShellsTypes = new List<Type>()
        {
            typeof(SingleUnderline)
        };
        public string GetPrefix()
        {
            return Prefix;
        }

        public string GetSuffix()
        {
            return Suffix;
        }

        public string RenderToHtml(string text)
        {
            return $"<strong>{text}</strong>";
        }

        public bool Contains(IShell shell)
        {
            return innerShellsTypes.Contains(shell.GetType());
        }
    }
}
