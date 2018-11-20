using System.Collections.Generic;

namespace Markdown
{
    public enum TokenType
    {
        Text,
        Italic,
        Bold
    }

    public class Token
    {
        //Текст - это токен;
        //Текст, окруженный разделителями - это токен;
        //Токен содержит вложенные токены;

        public Delimiter Delimiter;
        public List<Token> Tokens = new List<Token>();
        public Token ParentToken { get; private set; }
        public Token RootToken { get; private set; }
        public string Text { get; private set; }
        public bool Closed;
        public TokenType TokenType = TokenType.Text;


        public Token()
        {
            Tokens = new List<Token>();
            RootToken = this;
        }

        public Token(Delimiter delimiter)
        {
            Delimiter = delimiter;
            RootToken = this;
        }

        public void AddToken(Token token)
        {
            Tokens.Add(token);
            token.ParentToken = this;
            token.RootToken = RootToken;
        }

        public void AddText(string text)
        {
            var textToken = new Token {Text = text};
            AddToken(textToken);
        }
    }
}