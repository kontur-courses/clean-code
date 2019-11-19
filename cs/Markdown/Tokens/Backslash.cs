namespace Markdown.Tokens
{
    internal class Backslash : SpecSymbol
    {
        public Backslash() : base('\\') { }
        public override string ToText() => "\\";
    }
}
