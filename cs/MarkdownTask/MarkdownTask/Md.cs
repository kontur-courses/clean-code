using System;
using System.Collections.Generic;
using System.Text;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class Md
    {
        private const char Escape = '\\';
        private readonly Stack<Tag> outerTags = new Stack<Tag>();
        private readonly HashSet<char> triggerChars = new HashSet<char> { '_', '\\', '#' };

        private int currentPos;

        public string Render(string mdText)
        {
            if (mdText == null)
                throw new NullReferenceException("Text can't has null reference");

            var result = GetTagContent(mdText);

            if (outerTags.Count != 0)
            {
                var topTag = outerTags.Pop();
                result = topTag.OpeningPart + result + topTag.ClosingPart;
            }

            return result;
        }

        private string GetTagContent(string mdText)
        {
            var content = new StringBuilder();

            while (currentPos < mdText.Length)
            {
                var currentChar = mdText[currentPos];

                if (!triggerChars.Contains(currentChar))
                {
                    content.Append(mdText[currentPos]);
                    currentPos++;
                }
                else
                {
                    if (currentChar == Escape)
                    {
                        if (!triggerChars.Contains(mdText[currentPos + 1]))
                            content.Append(currentChar);

                        content.Append(mdText[currentPos + 1]);
                        currentPos += 2;
                        continue;
                    }

                    var tag = GetTag(mdText);

                    if (outerTags.Count == 0 || outerTags.Peek().Type != tag.Type)
                    {
                        outerTags.Push(tag);
                        tag.TagContent = GetTagContent(mdText);
                    }
                    else
                    {
                        outerTags.Pop();
                        content.Insert(0, tag.OpeningPart);
                        content.Append(tag.ClosingPart);
                    }

                    content.Append(tag.TagContent);
                }
            }

            return content.ToString();
        }

        private Tag GetTag(string mdText)
        {
            var sb = new StringBuilder();
            var currentChar = mdText[currentPos];

            while (triggerChars.Contains(currentChar) && currentChar != Escape)
            {
                sb.Append(currentChar);
                currentPos++;
                if (currentPos >= mdText.Length)
                    break;
                currentChar = mdText[currentPos];
            }

            var tagType = GetTagType(sb.ToString());
            return TagKeeper.GetHtmlTagByType(tagType);
        }

        private TagType GetTagType(string tag)
        {
            switch (tag)
            {
                case "_":
                    return TagType.SingleHighlight;

                case "__":
                    return TagType.DoubleHighlight;
                case "#":
                    return TagType.Header;

                default:
                    throw new ArgumentException("Unknown tag");
            }
        }
    }
}