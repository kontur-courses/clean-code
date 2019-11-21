using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class TagInserter
    {

        public string Insert(string text, List<Tag> toInsert)
        {
            toInsert.Sort((x, y) => x.Position.CompareTo(y.Position));
            StringBuilder outText = new StringBuilder();
            var currentTextIndex = 0;
            var currentTagsIndex = 0;
            var linkTagIndex = -1;
            while (currentTagsIndex < toInsert.Count)
            {
                var pair = toInsert[currentTagsIndex];
                outText.Append(text.Substring(currentTextIndex, pair.Position - currentTextIndex));
                currentTextIndex = pair.Position;
                switch (pair.CurrentType)
                {
                    case TagType.Em:
                        currentTextIndex++;
                        outText.Append("<em>");
                        break;
                    case TagType.EmClose:
                        currentTextIndex++;
                        outText.Append("</em>");
                        break;
                    case TagType.Strong:
                        currentTextIndex += 2;
                        outText.Append("<strong>");
                        break;
                    case TagType.StrongClose:
                        currentTextIndex += 2;
                        outText.Append("</strong>");
                        break;
                    case TagType.S:
                        currentTextIndex += 2;
                        outText.Append("<s>");
                        break;
                    case TagType.SClose:
                        currentTextIndex += 2;
                        outText.Append("</s>");
                        break;
                    case TagType.Backslash:
                        currentTextIndex++;
                        break;
                    case TagType.A:
                        currentTextIndex++;
                        outText.Append("<a");
                        linkTagIndex = outText.Length;
                        outText.Append(">");
                        break;
                    case TagType.AClose:
                        currentTextIndex++;
                        outText.Append("</a>");
                        break;
                    case TagType.LinkBracket:
                        currentTextIndex++;
                        while (toInsert[currentTagsIndex].CurrentType != TagType.LinkBracketClose)
                            currentTagsIndex++;
                        outText.Insert(linkTagIndex, " href=" + text.Substring(currentTextIndex, toInsert[currentTagsIndex].Position - currentTextIndex));
                        currentTextIndex = toInsert[currentTagsIndex].Position;
                        currentTextIndex++;
                        break;
                    case TagType.LinkBracketClose:
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
