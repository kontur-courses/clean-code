using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public class HeaderTagParser : ITagParser
    {
        private readonly string mdTag = "# ";
        public ICollection<Token> Parse(string markdown)
        {
            var start = 0;
            var tokens = new List<Token>();
            foreach (var paragraph in markdown.Split('\n'))
            {
                var length = paragraph.Length;
                if (paragraph.IndexOf(mdTag) == 0)
                {

                    tokens.Add(new Token(TagInfo.TagType.Header, start, TagInfo.Tag.Open, 2));
                    tokens.Add(new Token(TagInfo.TagType.Header, start + length, TagInfo.Tag.Close,0));
                }
                start += length;
            }
            return tokens;
        }
    }
}