namespace Markdown.Engine.Tokens
{
    public class TokenWithUnderScore: IToken
    {
        public string Value => "_";
        public TokenType TokenType { get; }

        public bool CanParse(string symbol) => symbol == Value;

        public IToken Create(string[] text, int index) => 
            NextSymbolIsTheSame(index, text, "_") ? new TokenStrong(): new TokenItalics();

        private bool NextSymbolIsTheSame(int index, string[] text, string symbol) => 
            index + 1 < text.Length && symbol == text[index + 1];
    }
}