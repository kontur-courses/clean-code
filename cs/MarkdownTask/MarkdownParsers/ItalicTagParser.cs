using FluentAssertions.Equivalency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MarkdownTask.TagInfo;

namespace MarkdownTask
{
    public class ItalicTagParser : ITagParser
    {
        private readonly char tag = '_';
        //private string[] escapeTags = { "__" };
        //private string htmlTag = "em";

        public List<Token> Parse(string text)
        {
            var tokens = new List<Token>();
            int start = -1;
            bool inTag = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '_')
                {
                    if (start == -1)
                    {
                        if (i == 0 || text[i - 1] == ' ')
                        {
                            start = i;
                            inTag = true;
                        }
                    }
                    else
                    {
                        if (i == text.Length - 1 || text[i + 1] == ' ')
                        {
                            var token = new Token(TagInfo.TagType.Italic,start,Tag.Open, i - start + 1);
                            tokens.Add(token);
                            start = -1;
                            inTag = false;
                        }
                    }
                }
                else if (inTag && char.IsDigit(text[i]))
                {
                    start = -1;
                    inTag = false;
                }
            }

            return tokens;
        }

        //public ICollection<Token> Parse(string markdown)
        //{
        //    var tokens = new List<Token>();
        //    var opened = new Stack<int>();

        //    for (int i = 0; i < markdown.Length - 1; i++)
        //    {
        //        if (markdown[i] == '\n')
        //        {
        //            opened = new Stack<int>();
        //            continue;
        //        }
        //        if (markdown[i] == '\\')
        //        {
        //            i++;
        //            continue;
        //        }

        //        if (markdown[i] == tag && !char.IsNumber(markdown[i + 1]))
        //        {
        //            if (opened.Any())
        //            {
        //                var o = opened.Pop();
        //                if (i - o <= 1)
        //                    continue;
        //                tokens.Add(new Token(TagInfo.TagType.Italic, o, TagInfo.Tag.Open, 1));
        //                tokens.Add(new Token(TagInfo.TagType.Italic, i, TagInfo.Tag.Close, 1));
        //            }
        //            else
        //                opened.Push(i);
        //        }
        //    }

        //    if (opened.Any() && markdown.Last() == '_')
        //    {
        //        var o = opened.Pop();
        //        if (markdown.Length - 1 - o > 1)
        //        {
        //            tokens.Add(new Token(TagInfo.TagType.Italic, o, TagInfo.Tag.Open, 1));
        //            tokens.Add(new Token(TagInfo.TagType.Italic, markdown.Length - 1, TagInfo.Tag.Close, 1));
        //        }
        //    }

        //    return tokens;
        //}
    }
}