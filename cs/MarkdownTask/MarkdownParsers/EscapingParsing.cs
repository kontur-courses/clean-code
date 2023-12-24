using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MarkdownTask.MarkdownParsers
{
    public class EscapingParsing : ITagParser
    {
        public ICollection<Token> Parse(string markdown)
        {
            var tokens = new List<Token>();

            string escaped = "\\_#";

            for (int i = 0; i < markdown.Length - 1; i++)
            {

                if (markdown[i] == '\\' && escaped.Contains(markdown[i+1]))
                {
                    tokens.Add(new Token(TagInfo.TagType.Empty, i, TagInfo.Tag.Open, 1));
                    i++;
                }
            }
            return tokens;
        }
    }
}
