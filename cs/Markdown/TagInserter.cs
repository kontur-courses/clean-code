using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    class TagInserter
    {
        public TagInserter()
        {

        }

        public string Insert(string text, Dictionary<int, Tag> toInsert)
        {
            StringBuilder outText = new StringBuilder();
            var pairs = toInsert.ToList().OrderBy(a=>a.Key).ToList();//временный костыль, потом перепилю
            int currentTextIndex = 0;
            int currentTagsIndex = 0;
            int linkTagIndex = -1;
            while(currentTagsIndex<pairs.Count)
            {
                var pair = pairs[currentTagsIndex];
                outText.Append(text.Substring(currentTextIndex, pair.Key - currentTextIndex));
                currentTextIndex = pair.Key;
                switch (pair.Value)
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
                        while (pairs[currentTagsIndex].Value != Tag.LinkBracketClose)
                            currentTagsIndex++;
                        outText.Insert(linkTagIndex, " href="+ text.Substring(currentTextIndex, pairs[currentTagsIndex].Key - currentTextIndex));
                        currentTextIndex = pairs[currentTagsIndex].Key;
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
