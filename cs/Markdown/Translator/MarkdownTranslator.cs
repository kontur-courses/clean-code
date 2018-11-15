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
        private int pointer;

        public MarkdownTranslator()
        {
            tagsCollection = new List<MarkdownTag>
            {
                new Italic(),
                new Bold()
            };
            tagsNesting = new Stack<MarkdownTag>();
            pointer = 0;
        }

        public MarkdownTranslator(IReadOnlyCollection<MarkdownTag> tagsCollection)
        {
            this.tagsCollection = tagsCollection;
            tagsNesting = new Stack<MarkdownTag>();
            pointer = 0;
        }

        public string Translate(string text)
        {
            var result = new StringBuilder();
            for (; pointer < text.Length; pointer++)
            {
                var currentTag = GetTag(text, pointer);
                if (currentTag != default(MarkdownTag))
                {
                    if (tagsNesting.Any())
                    {
                        var previousTag = tagsNesting.Peek();
                        if (currentTag != previousTag && previousTag.CanContain(currentTag))
                            result.Append(ParseTagOpening(currentTag));
                        else
                            result.Append(ParseTagEnding(tagsNesting.Pop()));
                    }
                    else if (HasCorrectEnding(currentTag.Tag, text, pointer + currentTag.Length))
                        result.Append(ParseTagOpening(currentTag));
                }
                else
                    result.Append(text[pointer]);
            }

            return result.ToString();
        }

        private string ParseTagOpening(MarkdownTag tag)
        {
            pointer += tag.Length - 1;
            tagsNesting.Push(tag);
            return tag.OpenTagTranslation;
        }

        private string ParseTagEnding(MarkdownTag tag)
        {
            pointer += tag.Length - 1;
            return tag.CloseTagTranslation;
        }

        private MarkdownTag GetTag(string line, int index)
        {
            var possibleTags = tagsCollection
                .Where(tag => tag.StartsWith(line[index]));

            if (tagsNesting.Any(tag => tag.Tag == line.Substring(index, tag.Length)))
                return tagsNesting
                    .First(tag => HasCorrectEnding(tag.Tag, line, index));

            return possibleTags.FirstOrDefault(t => IsCorrectTagOpening(t.Tag, line, index));
        }

        private bool IsCorrectTagOpening(string tag, string line, int startIndex)
        {
            if (startIndex + tag.Length >= line.Length)
                return false;
            if (startIndex != 0 && line.ElementAt(startIndex - 1) == '\\')
                return false;

            var nextChar = line.ElementAt(startIndex + tag.Length);
            return
                 line.Substring(startIndex, tag.Length) == tag &&
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