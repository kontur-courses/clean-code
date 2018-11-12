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

        public Md()
        {
            tags = new List<Tag>
            {
                new Tag("_", "_", "<em>", "</em>")
            };
        }

        public string Render(string rowString)
        {
            markdownString = rowString;
            lastIndex = 0;
            return ParseWithoutRegexp();
        }

        private string ParseWithoutRegexp()
        {
            var builder = new StringBuilder();
            var tokens = new List<Token>();
            var wasEscape = false;

            for (var i = 0; i < markdownString.Length; i++)
            {
                var found = false;
                if (markdownString[i] == '/')
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
                foreach (var token in tokens)
                {
                    if (markdownString[i] == token.Tag.MarkdownEnd[0])
                    {
                        token.EndIndex = i;
                        lastIndex = i + token.Tag.MarkdownEnd.Length;
                        builder.Append(token.Assembly(markdownString));
                        found = true;
                        break;
                    }
                }

                tokens = tokens.Where(t => t.EndIndex == 0).ToList();

                if (found)
                    continue;

                //проверяем на тег
                foreach (var tag in tags)
                {

                    if (markdownString[i] == tag.MarkdownStart[0])
                    {
                        builder.Append(markdownString.Substring(lastIndex, i - lastIndex));
                        tokens.Add(new Token(tag, i));
                    }
                }
            }

            builder.Append(markdownString.Substring(lastIndex));
            return builder.ToString();
        }
    }
}
