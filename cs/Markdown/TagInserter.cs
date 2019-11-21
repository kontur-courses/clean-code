using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentAssertions.Common;

namespace Markdown
{
    class TagInserter
    {
        public TagInserter()
        {

        }

        public string Insert(string text, List<(int, Tag)> toInsert)
        {
            toInsert.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            StringBuilder outText = new StringBuilder();
            int currentTextIndex = 0;
            int currentTagsIndex = 0;
            int linkTagIndex = -1;
            while(currentTagsIndex<toInsert.Count)
            {
                var pair = toInsert[currentTagsIndex];
                outText.Append(text.Substring(currentTextIndex, pair.Item1 - currentTextIndex));
                currentTextIndex = pair.Item1;
                switch (pair.Item2)
                {
                    case Tag.Em:
                        currentTextIndex++;
                        outText.Append("<em>");
                        break;
                    case Tag.EmClose:
                        currentTextIndex++;
                        outText.Append("</em>");
                        break;
                    case Tag.Strong:
                        currentTextIndex += 2;
                        outText.Append("<strong>");
                        break;
                    case Tag.StrongClose:
                        currentTextIndex += 2;
                        outText.Append("</strong>");
                        break;
                    case Tag.S:
                        currentTextIndex += 2;
                        outText.Append("<s>");
                        break;
                    case Tag.SClose:
                        currentTextIndex += 2;
                        outText.Append("</s>");
                        break;
                    case Tag.Backslash:
                        currentTextIndex ++;
                        break;
                    case Tag.A:
                        currentTextIndex++;
                        outText.Append("<a");
                        linkTagIndex = outText.Length;
                        outText.Append(">");
                        break;
                    case Tag.AClose:
                        currentTextIndex++;
                        outText.Append("</a>");
                        break;
                    case Tag.LinkBracket:
                        currentTextIndex++;
                        while (toInsert[currentTagsIndex].Item2 != Tag.LinkBracketClose)
                            currentTagsIndex++;
                        outText.Insert(linkTagIndex, " href="+ text.Substring(currentTextIndex, toInsert[currentTagsIndex].Item1 - currentTextIndex));
                        currentTextIndex = toInsert[currentTagsIndex].Item1;
                        currentTextIndex++;// скипнули закрывающую скобку
                        break;
                    case Tag.LinkBracketClose:
                        currentTextIndex++;
                        break;
                }

                currentTagsIndex++;
            }
            outText.Append(text.Substring(currentTextIndex));
            return outText.ToString();
        }
    }
}
