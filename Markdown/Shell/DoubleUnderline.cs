using System;
using System.Collections.Generic;
namespace Markdown.Shell
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
        public bool IsRestricted(string text, int startSuffix)
        {
            return GetSuffix().IsSubstring(text, startSuffix);
        }
    }
}
