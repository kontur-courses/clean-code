using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models.Tags;
using Markdown.Models.Tags.MdTags;

namespace Markdown.Models.Syntax
{
    internal class Syntax : ISyntax
    {
        private readonly Tag escapedTag;
        private readonly Dictionary<string, Type> tagsToTagTypes;

        private readonly Dictionary<Type, HashSet<Type>> tagTypesToAllowableInners =
            new Dictionary<Type, HashSet<Type>>()
            {
                {typeof(Underscore),new HashSet<Type>()},
                {typeof(DoubleUnderscore),new HashSet<Type>(){typeof(Underscore)}},
                {typeof(Plus),new HashSet<Type>(){typeof(Underscore),typeof(DoubleUnderscore) }},
                {typeof(Sharp),new HashSet<Type>(){typeof(Underscore),typeof(DoubleUnderscore) }}
            };

        private readonly HashSet<string> allTags;

        public Syntax(List<Tag> tags, Tag escapedTag)
        {
            this.escapedTag = escapedTag;
            tags.Add(escapedTag);

            allTags = tags
                .SelectMany(tag => new[] { tag.Opening, tag.Closing })
                .Where(tag => !string.IsNullOrEmpty(tag))
                .ToHashSet();

            tagsToTagTypes = tags.Select(tag => (tag.Opening, tag.GetType()))
                .ToDictionary(pair => pair.Opening, pair => pair.Item2);
        }

        public bool TryGetTag(string text, int position, out TagInfo tagInfo)
        {
            var possibleTag = string.Empty;
            var previousIsEscaped = PreviousSymbolIsEscaped(position, text);

            tagInfo = null;
            for (var i = position; i < text.Length; i++)
            {
                if (!allTags.Any(tag => tag.StartsWith(possibleTag + text[i])))
                    break;

                possibleTag += text[i];

                if (previousIsEscaped &&
                    allTags.Contains(possibleTag + text[i]))
                    break;
            }

            if (allTags.Contains(possibleTag))
            {
                var tagObj = tagsToTagTypes[possibleTag]
                    .GetConstructor(new Type[] { })?.Invoke(null);
                tagInfo = new TagInfo(tagObj as Tag, position,
                    possibleTag.Length, previousIsEscaped);
            }

            return tagInfo != null;
        }

        private bool PreviousSymbolIsEscaped(int position, string text)
        {
            return position != 0 && text[position - 1].ToString() == escapedTag.Opening;
        }

        public bool IsEscapingTag(Tag tag)
        {
            return tag.GetType() == escapedTag.GetType();
        }

        public bool IsStartParagraphTag(Tag tag)
        {
            var tagType = tagsToTagTypes[tag.Opening];
            return tagType == typeof(Sharp) || tagType == typeof(Plus);
        }

        public bool IsValidAsOpening(TagInfo tag, string text)
        {
            var length = tag.TagLength;
            if (tag.Position == 0)
                return !char.IsWhiteSpace(text[length]);

            if (IsStartParagraphTag(tag.Tag))
                return false;

            if (tag.Position + length == text.Length)
                return false;
            return !IsInWordWithDigits(tag, text) &&
                   !char.IsWhiteSpace(text[tag.Position + length]);
        }

        public bool IsValidAsClosing(TagInfo tag, string text)
        {
            var length = tag.TagLength;
            if (tag.Position == 0)
                return false;
            if (tag.Position + length == text.Length)
                return !char.IsWhiteSpace(text[tag.Position - 1]);
            return !IsInWordWithDigits(tag, text) &&
                   !char.IsWhiteSpace(text[tag.Position - 1]);
        }

        public bool IsValidAsInner(Tag source, Tag possibleInner)
        {
            var sourceType = source.GetType();
            var possibleInnerType = possibleInner.GetType();
            return tagTypesToAllowableInners.ContainsKey(sourceType) &&
                   tagTypesToAllowableInners[sourceType].Contains(possibleInnerType);
        }

        public bool IsInWord(PairedTag tag, string text)
        {
            var tagLength = tag.SourceTag.Closing.Length;
            if (tag.Position == 0 || tag.Position + tagLength == text.Length)
                return false;
            return !char.IsWhiteSpace(text[tag.Position]) && !char.IsWhiteSpace(text[tag.Position + tagLength]);
        }

        public bool IsInWord(TagInfo tag, string text)
        {
            return IsInWord(new PairedTag(tag.Tag, tag.Position), text);
        }

        private bool IsInWordWithDigits(TagInfo tag, string text)
        {
            return IsInWord(tag, text) &&
                   ((char.IsDigit(text[tag.Position - 1]) &&
                     char.IsLetterOrDigit(text[tag.Position + tag.TagLength])) ||
                    (char.IsLetterOrDigit(text[tag.Position - 1]) &&
                     char.IsDigit(text[tag.Position + tag.TagLength])));
        }
    }
}
