using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly List<Tag> tags;
        private string markdownString;
        private int lastIndex;
        private StringBuilder builder;
        private int i;

        public Md()
        {
            tags = new List<Tag>
            {
                new Tag("_", "_", "<em>", @"<\em>"),
                new Tag("__", "__", "<strong>", @"<\strong>")
            };
        }

        public string Render(string rowString)
        {
            markdownString = rowString;
            lastIndex = 0;
            i = 0;
            builder = new StringBuilder();
            return ParseWithoutRegexp();
        }

        private string ParseWithoutRegexp()
        {
            var tokens = new List<Token>();
            var wasEscape = false;

            for (; i < markdownString.Length; i++)
            {
                var found = false;
                if (markdownString[i] == '\\')
                {
                    wasEscape = true;
                    builder.Append(markdownString.Substring(lastIndex, i - lastIndex));
                    lastIndex = i + 1;
                    continue;
                }

                if (wasEscape)
                {
                    wasEscape = false;
                    continue;
                }

                //проверяем чтобы закрыть токен
                if (tokens.Count != 0)
                    found = FindEndToken(tokens);
                //foreach (var token in tokens)
                //{
                //    if (markdownString[i] == token.Tag.MarkdownEnd[0])
                //    {
                //        token.EndIndex = i + token.Tag.MarkdownEnd.Length - 1;
                //        lastIndex = i + token.Tag.MarkdownEnd.Length;
                //        builder.Append(token.Assembly(markdownString));
                //        found = true;
                //        break;
                //    }
                //}

                tokens = tokens.Where(t => t.EndIndex == 0).ToList();

                if (found)
                    continue;

                //проверяем на тег
                var newToken = FindToken();
                if (newToken != null)
                {
                    tokens.Add(newToken);
                }


            }

            if (lastIndex < markdownString.Length)
                builder.Append(markdownString.Substring(lastIndex));
            return builder.ToString();
        }

        public Token FindToken()
        {
            var possibleTags = tags.Where(tag => markdownString[i] == tag.MarkdownStart[0]).ToList();

            if (possibleTags.Count == 0)
                return null;
            
            builder.Append(markdownString.Substring(lastIndex, i - lastIndex));

            for (var j = 1; possibleTags.Count > 1; j++)
            {
                var posTags = new List<Tag>();
                var shortTags = new List<Tag>();
                foreach (var tag in possibleTags)
                {
                    if (tag.MarkdownStart.Length <= j)
                        shortTags.Add(tag);
                    if (tag.MarkdownStart.Length > j && tag.MarkdownStart[j] == markdownString[i + j])
                    {
                        posTags.Add(tag);
                    }
                }

                if (posTags.Count == 0)
                    posTags = shortTags;

                possibleTags = posTags;
            }

            if (possibleTags.Count != 1)
                return null;

            var startIndex = i;
            i = i + possibleTags[0].MarkdownStart.Length - 1;
            return new Token(possibleTags[0], startIndex);
            
        }
        public bool FindEndToken(List<Token> tokens)
        {
            var possibleTokens = tokens.Where(token => markdownString[i] == token.Tag.MarkdownEnd[0]).ToList();

            if (possibleTokens.Count == 0)
                return false;

            for (var j = 1; possibleTokens.Count > 1; j++)
            {
                var posTokens = new List<Token>();
                var shortTokens = new List<Token>();
                foreach (var token in possibleTokens)
                {
                    if (token.Tag.MarkdownEnd.Length <= j)
                        shortTokens.Add(token);
                    if (token.Tag.MarkdownEnd.Length > j && token.Tag.MarkdownEnd[j] == markdownString[i + j])
                    {
                        posTokens.Add(token);
                    }
                }

                if (posTokens.Count == 0)
                    posTokens = shortTokens;

                possibleTokens = posTokens;
            }

            if (possibleTokens.Count != 1)
                return false;

            //end index мкеньше
            var possibleToken = possibleTokens[0];
            possibleToken.EndIndex = i + possibleToken.Tag.MarkdownEnd.Length - 1;
            lastIndex = i + possibleToken.Tag.MarkdownEnd.Length;
            builder.Append(possibleToken.Assembly(markdownString));
            i = i + possibleToken.Tag.MarkdownEnd.Length - 1;
            return true;
        }
    }
}
