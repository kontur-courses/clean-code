using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public Delimiter StartingDelimiter;
        public Delimiter ClosingDelimiter;
        public List<Token> tokens = new List<Token>();
        public Token parentToken;
        public Token rootToken;
        public string text { get; private set; }

        public Token()
        {
            tokens = new List<Token>();
        }

        public Token(Delimiter startingDelimiter)
        {
            this.StartingDelimiter = startingDelimiter;
            rootToken = this;
        }

        public void AddToken(Token token)
        {
            tokens.Add(token);
            token.parentToken = this;
            token.rootToken = this.rootToken;
        }

        public void InsertToken(int position, Token token)
        {
            tokens.Insert(position, token);
            token.parentToken = this;
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