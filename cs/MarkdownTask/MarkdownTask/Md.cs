using System;
using System.Collections.Generic;
using System.Text;
using MarkdownTask.Extensions;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class Md
    {
        private const char Escape = '\\';
        private readonly Stack<Tag> openedTags = new Stack<Tag>();
        private readonly HashSet<char> triggerChars = new HashSet<char> { '_', '\\', '#' };

        private int currentPos;

        public string Render(string mdText)
        {
            if (mdText == null)
                throw new NullReferenceException("Text can't has null reference");

            var result = GetTagContent(mdText);          

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

                    var tag = BuildTag(mdText);

                    if (IsTagOpening(tag))
                    {
                        openedTags.Push(tag);
                        tag.TagContent = GetTagContent(mdText);
                    }
                    else
                    {
                        openedTags.Pop();
                        content = content.WrapContentToTag(tag);
                    }

                    content.Append(tag.TagContent);
                }
            }

            if (openedTags.Count != 0)
            {
                var topTag = openedTags.Pop();
                content = content.WrapContentToTag(topTag);
            }

            return content.ToString();
        }

        private bool IsTagOpening(Tag tag)
        {
            return openedTags.Count == 0 || openedTags.Peek().Type != tag.Type;
        }

        private Tag BuildTag(string mdText)
        {
            var builder = new StringBuilder();
            var currentChar = mdText[currentPos];

            while (triggerChars.Contains(currentChar) && currentChar != Escape)
            {
                builder.Append(currentChar);
                currentPos++;
                if (currentPos >= mdText.Length)
                    break;
                currentChar = mdText[currentPos];
            }

            var tagType = GetTagType(builder.ToString());
            return TagKeeper.GetHtmlTagByType(tagType);
        }

        private TagType GetTagType(string tag)
        {
            return tag switch
            {
                "_" => TagType.Italic,
                "__" => TagType.Bold,
                "#" => TagType.Header,
                _ => throw new ArgumentException("Uknown tag")
            };
        }
    }
}