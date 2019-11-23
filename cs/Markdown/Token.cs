using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public readonly StringBuilder Value = new StringBuilder();
        public readonly Deque<Token> InnerTokens = new Deque<Token>();
        public readonly string Prefix;
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
            if (InnerTokens.IsEmpty)
                Value.Append(anyChar);
            else
            {
                if (InnerTokens.Last.IsClosed)
                    InnerTokens.AddLast(new Token(""));
                InnerTokens.Last.AddChar(anyChar);
            }
        }

        public Token CreateInnerToken() => CreateInnerToken("");

        public Token CreateInnerToken(string prefix)
        {
            var innerToken = new Token(prefix);
            InnerTokens.AddLast(innerToken);

            return innerToken;
        }

        public void CloseToken()
        {
            IsClosed = true;
        }


        public void ClearTags(HashSet<string> notAllowedTag, string hideTag)
        {
            foreach (var token in InnerTokens)
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