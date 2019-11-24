using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Token
    {
        private readonly StringBuilder value = new StringBuilder();
        public string GetValue => value.ToString();
        private readonly Deque<Token> innerTokens = new Deque<Token>();
        public IEnumerable<Token> GetInnerTokens => innerTokens;
        public string Prefix { get;}
        public bool IsClosed { get; private set; }
        public TokenPrefixCondition Condition { get; private set; }

        public Token(string prefix)
        {
            Prefix = prefix;
            IsClosed = false;
            Condition = TokenPrefixCondition.Tag;
        }

        public Token() : this("")
        {
        }

        public void AddChar(char anyChar)
        {
            if (innerTokens.IsEmpty)
                value.Append(anyChar);
            else
            {
                if (innerTokens.Last.IsClosed)
                    innerTokens.AddLast(new Token(""));
                innerTokens.Last.AddChar(anyChar);
            }
        }

        public Token CreateInnerToken() => CreateInnerToken("");

        public Token CreateInnerToken(string prefix)
        {
            var innerToken = new Token(prefix);
            innerTokens.AddLast(innerToken);

            return innerToken;
        }

        public void CloseToken()
        {
            IsClosed = true;
        }


        public void ClearTags(HashSet<string> notAllowedTag, string hideTag)
        {
            foreach (var token in innerTokens)
            {
                if (token.Prefix == hideTag)
                {
                    token.Condition = TokenPrefixCondition.Hide;
                    token.ClearTags(notAllowedTag, hideTag);
                }
                else if (notAllowedTag.Contains(token.Prefix))
                {
                    token.Condition = TokenPrefixCondition.Symbols;
                    token.ClearTags(notAllowedTag, hideTag);
                }
            }
        }
    }
}