using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum TokenType
    {
        italic,
        bold,
        text,
        escaped
    }

    public class Token
    {
        //Текст - это токен;
        //Текст, окруженный разделителями - это токен;
        //Токен содержит вложенные токены;
        public Delimiter StartingDelimiter;
        public Delimiter ClosingDelimiter;
        public List<Token> tokens = new List<Token>();
        public Token ParentToken { get; private set; }
        public Token RootToken { get; private set; }
        public string text { get; private set; }
        public bool closed;
        public TokenType tokenType = TokenType.text;


        public Token()
        {
            tokens = new List<Token>();
            RootToken = this;
        }

        public Token(Delimiter startingDelimiter)
        {
            StartingDelimiter = startingDelimiter;
            RootToken = this;
        }

        public void AddToken(Token token)
        {
            tokens.Add(token);
            token.ParentToken = this;
            token.RootToken = this.RootToken;
        }

        public void InsertToken(int position, Token token)
        {
            tokens.Insert(position, token);
            token.ParentToken = this;
        }

        public void AddText(string text)
        {
            var textToken = new Token {text = text};
            AddToken(textToken);
        }

        public void InsertText(int position, string text)
        {
            var textToken = new Token {text = text};
            InsertToken(position, textToken);
        }
    }
}