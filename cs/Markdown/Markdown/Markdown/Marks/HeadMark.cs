using System;

namespace Markdown
{
    public class HeadMark : Mark
    {
        public HeadMark()
        {
            DefiningSymbol = "#";
            AllSymbols = new[] {"#", Environment.NewLine};
            FormattedMarkSymbols = ("\\<h1>", "\\</h1>");
        }
    }
}