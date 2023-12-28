namespace Markdown.Parsers
{
    public class Token
    {
        public int Index;
        public TokenType Type;

        public int Offset => Tokenizer.TypeToSymbols[Type].Length;
    }

    public enum TokenType
    {
        Bold,
        Italic,
        Header,
        Escape,
    }
}
