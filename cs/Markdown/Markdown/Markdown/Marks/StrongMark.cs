namespace Markdown
{
    public class StrongMark : Mark
    {
        public StrongMark()
        {
            DefiningSymbol = "__";
            AllSymbols = new[] {"__", "__"};
            FormattedMarkSymbols = ("\\<strong>", "\\</strong>");
        }
    }
}