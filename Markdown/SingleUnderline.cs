using System.Collections.Generic;

namespace Markdown
{
    public class SingleUnderline : IShell
    {
        private const string Prefix = "_";
        private const string Suffix = "_";
        private List<IShell> innerShells = new List<IShell>()
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
            throw new System.NotImplementedException();
        }
    }
}
