using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenTree
    { 
        private readonly int weight;
        public List<TokenTree> Children { get; } = new();
        public TokenType TokenType { get; }
        public string Value { get; }
        
        public  int Count => weight + Children.Sum(child => child.Count);
        
        public TokenTree(IToken token)
        {
            TokenType = token.TokenType;
            Value = token.Value;
            weight = 2;
        }

        public TokenTree(IToken token, List<TokenTree> children) : this(token)
        {
            Children = children;
        }

        public TokenTree(string value)
        {
            Value = value;
            TokenType = TokenType.Text;
            weight = 1;
        }
        
        public TokenTree(string value, int weight)
        {
            Value = value;
            TokenType = TokenType.Text;
            this.weight = weight;
        }

        public TokenTree(IToken token, int weight)
        {
            TokenType = token.TokenType;
            Value = token.Value;
            this.weight = weight;
        }
   
        public void Add(TokenTree component)
        {
            Children.Add(component);
        }

        protected bool Equals(TokenTree other) => 
                TokenType == other.TokenType && Children.SequenceEqual(other.Children);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TokenTree)obj);
        }

        public override int GetHashCode() => HashCode.Combine(TokenType, Value, Children);
    }
}