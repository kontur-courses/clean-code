namespace Markdown
{
    public class LinkMark : Mark
    {
        public string LinkText { get; set; }
        public string Link { get; set; }
        public LinkMark()
        {
            DefiningSymbol = "[";
            AllSymbols = new[] {"[","]","(",")"};
            FormattedMarkSymbols = ("","");
        }
    }
}