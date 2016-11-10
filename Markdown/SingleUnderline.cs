using System;
using System.Collections.Generic;

namespace Markdown
{
    public class SingleUnderline : IShell
    {
        private const string Prefix = "_";
        private const string Suffix = "_";
        private readonly List<Type> innerShellsTypes = new List<Type>()
        {
            
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
            return $"<em>{text}</em>";
        }

        public bool Contains(IShell shell)
        {
            return innerShellsTypes.Contains(shell.GetType());
        }
    }
}
