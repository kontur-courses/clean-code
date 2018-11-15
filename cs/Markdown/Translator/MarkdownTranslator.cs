using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;
using Markdown.Tag.Standart;

namespace Markdown.Translator
{
    public class MarkdownTranslator : IMarkdownTranslator
    {
        private readonly IReadOnlyCollection<MarkdownTag> tagsCollection;
        private readonly Stack<MarkdownTag> tagsNesting;
        private readonly StringBuilder sb;
        private int index;

        public MarkdownTranslator()
        {
            tagsCollection = new List<MarkdownTag>
            {
                new Italic(),
                new Bold()
            };
            tagsNesting = new Stack<MarkdownTag>();
            sb = new StringBuilder();
            index = 0;
        }

        public MarkdownTranslator(IReadOnlyCollection<MarkdownTag> tagsCollection)
        {
            this.tagsCollection = tagsCollection;
            tagsNesting = new Stack<MarkdownTag>();
            sb = new StringBuilder();
            index = 0;
        }

        public string Translate(string text)
        {
            for (; index < text.Length; index++)
            {
                var currentTag = GetTag(text, index);
                if (currentTag != default(MarkdownTag))
                {
                    if (tagsNesting.Any())
                    {
                        var previousTag = tagsNesting.Peek();
                        if (currentTag != previousTag && previousTag.CanContain(currentTag))
                            ParseTagOpening(currentTag);
                        else
                            ParseTagEnding(currentTag);
                    }
                    else if (HasCorrectEnding(currentTag.Tag, text, index + currentTag.Length))
                        ParseTagOpening(currentTag);
                }
                else
                    sb.Append(text[index]);
            }

            return sb.ToString();
        }

        private void ParseTagOpening(MarkdownTag tag)
        {
            sb.Append(tag.OpenTagTranslation);
            index += tag.Length - 1;
            tagsNesting.Push(tag);
        }

        private void ParseTagEnding(MarkdownTag tag)
        {
            sb.Append(tag.CloseTagTranslation);
            index += tag.Length - 1;
            tagsNesting.Pop();
        }

        private MarkdownTag GetTag(string line, int i)
        {
            var possibleTags = tagsCollection
                .Where(tag => tag.StartsWith(line[i]));

            if (tagsNesting.Any(tag => tag.Tag == line.Substring(i, tag.Length)))
                return tagsNesting
                    .First(tag => HasCorrectEnding(tag.Tag, line, i));

            return possibleTags.FirstOrDefault(t => IsCorrectTagOpening(t.Tag, line, i));
        }

        private bool IsCorrectTagOpening(string tag, string line, int index)
        {
            if (index + tag.Length >= line.Length)
                return false;
            if (index != 0 && line.ElementAt(index - 1) == '\\')
                return false;

            var nextChar = line.ElementAt(index + tag.Length);
            return
                 line.Substring(index, tag.Length) == tag &&
                char.IsLetter(nextChar);
        }

        private bool HasCorrectEnding(string tag, string line, int startIndex)
        {
            while (startIndex < line.Length)
            {
                startIndex = line.IndexOf(tag, startIndex, StringComparison.CurrentCulture);
                if (startIndex == -1)
                    return false;
                var previousChar = line.ElementAt(startIndex - 1);
                if (char.IsLetter(previousChar) &&
                    line.Substring(startIndex, tag.Length) == tag)
                    return true;
                startIndex++;
            }

            return false;
        }
    }
}