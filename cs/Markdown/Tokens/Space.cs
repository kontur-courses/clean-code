namespace Markdown.Tokens
{
    internal class Space : SpecSymbol
    {
        public Space() : base(' ') { }
        public override string ToText() => " ";
    }
}
