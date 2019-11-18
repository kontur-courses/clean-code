using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Token
    {
        private StringBuilder value = new StringBuilder();
        private Deque<Token> innerTokens = new Deque<Token>();
        public readonly string Prefix;
        private bool isClosed = false;
        private TokenPrefixCondition condition = TokenPrefixCondition.Tag;
        private string Tag;
        public int ActualEnd;

        public Token(string prefix)
        {
            Prefix = prefix;
        }

        public void AddChar(char anyChar)
        {
            if (innerTokens.IsEmpty)
                value.Append(anyChar);
            else
            {
                if (innerTokens.Last.isClosed)
                    innerTokens.AddLast(new Token(""));
                innerTokens.Last.AddChar(anyChar);
            }
        }

        public Token CreateInnerToken(string prefix)
        {
            var innerToken = new Token(prefix);
            innerTokens.AddLast(innerToken);

            return innerToken;
        }

        public void CloseToken(int endPosition, string tag)
        {
            isClosed = true;
            ActualEnd = endPosition;
            Tag = tag;
        }

        public string ConvertToHTMLTag()
        {
            if (innerTokens.IsEmpty)
            {
                return string.IsNullOrWhiteSpace(Tag)
                    ? value.ToString()
                    : $"<{Tag}>{value}</{Tag}>";
            }

            var htmlString = new StringBuilder();
            while (!innerTokens.IsEmpty)
            {
                htmlString.Append(innerTokens.RemoveFirst().ConvertToHTMLTag());
            }

            if (condition == TokenPrefixCondition.Hide)
                return htmlString.ToString();

            if (!isClosed)
                return Prefix + htmlString;
            return condition == TokenPrefixCondition.Tag
                ? $"<{Tag}>{htmlString}</{Tag}>"
                : $"{Prefix}{htmlString}{Prefix}";
        }

        public void ClearTags(HashSet<string> notAllowedTag, string hideTag)
        {
            foreach (var token in innerTokens)
            {
                if (token.Prefix == hideTag)
                {
                    token.condition = TokenPrefixCondition.Hide;
                    token.ClearTags(notAllowedTag, hideTag);
                }else if (notAllowedTag.Contains(token.Prefix))
                {
                    token.condition = TokenPrefixCondition.Symbols;
                    token.ClearTags(notAllowedTag, hideTag);
                }
            }
        }
    }
}