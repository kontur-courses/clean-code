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
        private string currentText;

        public MarkdownTranslator()
        {
            tagsCollection = new List<MarkdownTag>
            {
                new Italic(),
                new Bold()
            };
            tagsNesting = new Stack<MarkdownTag>();
        }

        public string Translate(string text)
        {
            currentText = text;
            return GetTranslation();
        }

        private string GetTranslation()
        {
            var translation = new StringBuilder();

            for (pointer = 0; pointer < currentText.Length; pointer++)
            {
                var currentTag = GetTagAtPointer();
                if (currentTag != null)
                {
                    translation.Append(ParseTag(currentTag));
                    pointer += currentTag.Length - 1;
                }
                else
                    translation.Append(currentText[pointer]);
            }

            return translation.ToString();
        }

        private string ParseTag(MarkdownTag tag)
        {
            return tagsNesting.Any()
                ? ParseNewOrPreviousTag(tag)
                : ParseNewTag(tag);
        }

        private string ParseNewOrPreviousTag(MarkdownTag currentTag)
        {
            var previousTag = tagsNesting.Peek();
            return currentTag
                .IsInnerTagOf(previousTag)
                ? ParseNewTag(currentTag)
                : ParsePreviousTag();
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

        private MarkdownTag GetTagAtPointer()
        {
            if (tagsNesting.Any())
            {
                var previousTag = tagsNesting.Peek();
                if (IsPointerAtTag(previousTag))
                    return previousTag;
            }

            return tagsCollection
                .FirstOrDefault(HasCorrectBounds);
        }

        private bool HasCorrectBounds(MarkdownTag tag)
        {
            if (!IsPointerAtTag(tag))
                return false;
            return HasCorrectOpening(tag, pointer)
                   && HasCorrectEnding(tag, pointer + tag.Length);
        }

        private bool HasCorrectOpening(MarkdownTag tag, int indexOfTag)
        {
            if (indexOfTag + tag.Length >= currentText.Length)
                return false;
            if (indexOfTag != 0 && currentText[indexOfTag - 1] == '\\')
                return false;

            var nextChar = currentText[indexOfTag + tag.Length];
            return char.IsLetter(nextChar);
        }

        private bool HasCorrectEnding(MarkdownTag tag, int startIndex)
        {
            while (startIndex < currentText.Length)
            {
                startIndex = currentText.IndexOf(tag.Value, startIndex, StringComparison.CurrentCulture);
                if (startIndex == -1)
                    return false;
                var previousChar = currentText[startIndex - 1];
                if (char.IsLetter(previousChar))
                    return true;
                startIndex++;
            }

            return false;
        }

        private bool IsPointerAtTag(MarkdownTag tag)
        {
            if (tag.Length + pointer > currentText.Length)
                return false;
            return currentText.Substring(pointer, tag.Length) == tag.Value;
        }
    }
}