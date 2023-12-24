using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarkdownTask.TagInfo;

namespace MarkdownTask
{
    public class StrongTagParser : ITagParser
    {
        private char tag = '_';
        public ICollection<Token> Parse(string markdown)
        {
            var tokens = new List<Token>();
            var opened = new Stack<int>();

            for (int i = 0; i < markdown.Length - 2; i++)
            {
                if (markdown[i] == '\n')
                {
                    opened = new Stack<int>();
                    continue;
                }
                if (markdown[i] == '\\')
                {
                    i++;
                    continue;
                }

                if (markdown[i] == tag && markdown[i + 1] == tag && !char.IsNumber(markdown[i + 2]))
                {
                    if (opened.Any())
                    {
                        var o = opened.Pop();
                        if (i - o <= 1)
                            continue;
                        tokens.Add(new Token(TagInfo.TagType.Strong, o, TagInfo.Tag.Open, 2));
                        tokens.Add(new Token(TagInfo.TagType.Strong, i, TagInfo.Tag.Close, 2));
                    }
                    else
                        opened.Push(i);
                }
            }

            if (markdown.Length >=2 && markdown[markdown.Length - 2] == tag && markdown[markdown.Length - 1] == tag)
            {
                if (opened.Any())
                {
                    var o = opened.Pop();
                    if (markdown.Length - 2 - o > 1)
                    {
                        tokens.Add(new Token(TagInfo.TagType.Strong, o, TagInfo.Tag.Open, 2));
                        tokens.Add(new Token(TagInfo.TagType.Strong, markdown.Length - 2, TagInfo.Tag.Close, 2));
                    }
                }
            }

            return tokens;
        }
    }
}