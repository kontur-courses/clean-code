namespace Markdown
{
    public class TokenLink : IToken
    {
        public TokenType TokenType => TokenType.Link;
        public string Value { get; private set;}

        public bool CanParse(string symbol) => false;

        public IToken Create(string[] text, int index)
        {
            var link = "";
            while (text[index] != ")")
            {
                link += text[index];
                index++;
            }

            return new TokenLink{Value = link};
        }
    }
}