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
        private readonly HashSet<char> tagsOpening;
        private int pointer;
        private string currentText;

        public MarkdownTranslator()
        {
            tagsCollection = new List<MarkdownTag>
            {
                new Italic(),
                new Bold()
            };
            tagsNesting = new Stack<MarkdownTag>();
            tagsOpening = new HashSet<char>();
            foreach (var markdownTag in tagsCollection)
                tagsOpening.Add(markdownTag.Value[0]);
        }

        public string Translate(string text)
        {
            currentText = text;
            pointer = 0;
            tagsNesting.Clear();
            return GetTranslation();
        }

        private string GetTranslation()
        {
            var translation = new StringBuilder();

            while (pointer < currentText.Length)
                translation.Append(
                    ParseTag(
                        DetermineTag(currentText[pointer])));

            return translation.ToString();
        }

        private bool IsTag(char c)
        {
            return tagsOpening.Contains(c);
        }

        private bool IsLetter(char c)
        {
            return !tagsOpening.Contains(c);
        }

        private string ParseTag(MarkdownTag tag)
        {
            if (tag is Text)
                return tag.Value;
            if (tagsNesting.Any())
            {
                var previousTag = tagsNesting.Peek();
                if (IsCorrectEnding(previousTag))
                    return ParsePreviousTag();
                if (previousTag != tag && !previousTag.CanContain(tag))
                    return tag.Value;
            }

            return ParseNewTag(tag);
        }

        private string ParseNewTag(MarkdownTag tag)
        {
            tagsNesting.Push(tag);
            return tag.GetTranslation();
        }

        private string ParsePreviousTag()
        {
            return tagsNesting
                .Pop()
                .GetTranslationWithBackslash();
        }

        private MarkdownTag DetermineTag(char c)
        {
            var tag = IsLetter(c)
                ? ReadUntil(IsLetter, pointer)
                : ReadUntil(IsTag, pointer);

            pointer += tag.Length;

            if (tagsNesting.Any())
            {
                var previousTag = tagsNesting.Peek();
                if (previousTag.Value == tag)
                    return previousTag;
            }

            var suitableTag = tagsCollection
                .FirstOrDefault(t => t.Value == tag);

            return HasCorrectBounds(suitableTag)
                ? suitableTag
                : new Text(tag);
        }

        private string ReadUntil(Func<char, bool> continuator, int startIndex)
        {
            var result = new StringBuilder();
            while (true)
            {
                result.Append(currentText[startIndex]);
                startIndex++;
                if (startIndex >= currentText.Length)
                    break;
                if (!continuator(currentText[startIndex]))
                    break;
            }

            return result.ToString();
        }

        private bool HasCorrectBounds(MarkdownTag tag)
        {
            if (tag == null)
                return false;
            return HasCorrectOpening(tag) && HasCorrectEnding(tag);
        }

        private bool HasCorrectOpening(MarkdownTag tag)
        {
            if (pointer >= currentText.Length)
                return false;
            if (pointer - tag.Length != 0 && currentText[pointer - tag.Length - 1] == '\\')
                return false;

            return char.IsLetter(currentText[pointer]);
        }

        private bool HasCorrectEnding(MarkdownTag tag)
        {
            var index = pointer;
            while (index < currentText.Length)
            {
                index = currentText.IndexOf(tag.Value, index, StringComparison.Ordinal);
                if (index == -1)
                    return false;
                var previousChar = currentText[index - 1];
                var tagValue = ReadUntil(IsTag, index);
                index += tagValue.Length;
                if (char.IsLetter(previousChar) && tagValue == tag.Value)
                    return true;
            }

            return false;
        }

        private bool IsCorrectEnding(MarkdownTag tag)
        {
            var previousChar = currentText[pointer - tag.Length - 1];
            var tagValue = ReadUntil(IsTag, pointer - tag.Length);
            return char.IsLetter(previousChar) && tagValue == tag.Value;
        }
    }
}