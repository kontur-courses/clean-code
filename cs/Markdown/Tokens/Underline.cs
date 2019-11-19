namespace Markdown.Tokens
{
    internal class Underline : SpecSymbol
    {
        public Underline() : base('_') { }

        public override string ToText() => "_";
    }
}
