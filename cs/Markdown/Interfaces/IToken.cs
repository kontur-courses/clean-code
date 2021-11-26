namespace Markdown
{
    public interface IToken
    {
        TokenType TokenType { get; }
        string Value { get; }
        public bool CanParse(string symbol) => symbol == Value;

        IToken Create(string[] text, int index);
    }
}