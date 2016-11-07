using System.Collections.Generic;

namespace Markdown
{
    class Shell
    {
        public readonly string HtmlPrefix;
        public readonly string HtmlSuffix;
        public readonly string Prefix;
        public readonly string Suffix;
        private readonly List<Shell> listSupporters;
        public Shell(string prefix, string suffix, string htmlPrefix, string htmlSuffix)
        {
            Suffix = suffix;
            HtmlPrefix = htmlPrefix;
            HtmlSuffix = htmlSuffix;
            Prefix = prefix;
            listSupporters = new List<Shell>();
        }

        public void AddShell(Shell shell)
        {
            listSupporters.Add(shell);
        }

        public bool IncludedInsideShell(Shell shell)
        {
            return listSupporters.Contains(shell);
        }
    }
}
