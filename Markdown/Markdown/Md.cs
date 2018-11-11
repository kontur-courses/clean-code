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

        public Md()
        {
            tags = new List<Tag>
            {
                new Tag("_", "_", "<em>", "</em>")
            };
        }

        public string Render(string rowString)
        {
            return ParseWithoutRegexp(rowString);
        }

        private string ParseWithoutRegexp(string rowString)
        {
            var builder = new StringBuilder();
            var tokens = new List<Token>();

            var lastIndex = 0;
            for (var i = 0; i < rowString.Length; i++)
            {
                var found = false;
                foreach (var token in tokens)
                {
                    if (rowString[i] == token.Tag.MarkdownEnd[0])
                    {
                        token.EndIndex = i;
                        lastIndex = i + token.Tag.MarkdownEnd.Length;
                        builder.Append(token.Assembly(rowString));
                        found = true;
                        break;
                    }
                }

                tokens = tokens.Where(t => t.EndIndex == 0).ToList();

                if (found)
                    continue;

                foreach (var tag in tags)
                {
                    if (rowString[i] == tag.MarkdownStart[0])
                    {
                        builder.Append(rowString.Substring(lastIndex, i - lastIndex));
                        tokens.Add(new Token(tag, i));
                    }
                }
            }


            builder.Append(rowString.Substring(lastIndex));

            return builder.ToString();
        }

    }
}
