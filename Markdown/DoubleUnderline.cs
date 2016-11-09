using System.Collections.Generic;

namespace Markdown
{
    public class DoubleUnderline : IShell
    {
        private const string Prefix = "__";
        private const string Suffix = "__";
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
            return $"<strong>{text}</strong>";
        }

        public bool Contains(IShell shell)
        {
            throw new System.NotImplementedException();
        }
    }
}
