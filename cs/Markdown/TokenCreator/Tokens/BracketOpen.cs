using System.Collections.Generic;

namespace Markdown
{
    public class Bracket : IToken
    {
        private HashSet<string> brakets = new() { "]", "[", ")", "(" };
        public TokenType TokenType { get; }
        public string Value { get; }

        public bool CanParse(string symbol) => brakets.Contains(symbol);

        public IToken Create(string[] text, int index)
        {
            if (text[index] == "]")
                return new SquareBracketClose();
            if (text[index] == "[")
                return new SquareBracketOpen();
            if (text[index] == ")")
                return new BracketClose();
            return new BracketOpen();
        }
    }
    
    public class SquareBracketClose : IToken
    {
        public TokenType TokenType => TokenType.SquareBracketClose;
        public string Value => "]";
        public IToken Create(string[] text, int index) => this;
    }
    
    public class SquareBracketOpen : IToken
    {
        public TokenType TokenType => TokenType.SquareBracketOpen;
        public string Value => "[";
        public IToken Create(string[] text, int index) => this;
    }
    
    public class BracketOpen : IToken
    {
        public TokenType TokenType => TokenType.BracketOpen;
        public string Value => "(";
        public IToken Create(string[] text, int index) => this;
    }
    
    public class BracketClose : IToken
    {
        public TokenType TokenType => TokenType.BracketClose;
        public string Value => ")";
        public IToken Create(string[] text, int index) => this;
    }
}