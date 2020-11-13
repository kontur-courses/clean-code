using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TextStyleTagCollector<TTag> : TagCollector<TTag>
        where TTag : Tag, new()
    {
        public TextStyleTagCollector(TextWorker textWorker)
            : base(textWorker)
        {
        }

        public override List<TTag> CollectTags(string line)
        {
            var collectedTags = new List<TTag>();
            var openingTagsIndexes = GetPotentialOpeningTag(line);
            var closingTagsIndexes = GetPotentialClosingTag(line);

            while (openingTagsIndexes.Any() && closingTagsIndexes.Any())
            {
                var indexOfOpeningTag = openingTagsIndexes.First();
                var suitableClosingTagIndex = closingTagsIndexes
                    .Where(index => index != indexOfOpeningTag)
                    .FirstOrDefault(index =>
                        IsValidTag(line, indexOfOpeningTag, index));

                if (suitableClosingTagIndex != 0)
                {
                    collectedTags.Add(ConstructTag(indexOfOpeningTag, suitableClosingTagIndex));
                    closingTagsIndexes.RemoveAll(index =>
                        index <= suitableClosingTagIndex);
                    openingTagsIndexes.RemoveAll(index =>
                        index <= suitableClosingTagIndex);
                }
                else
                    openingTagsIndexes.Remove(indexOfOpeningTag);
            }

            return collectedTags;
        }

        private List<int> GetPotentialOpeningTag(string line)
        {
            var potentialOpeningTags = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (IsStartOfOpeningTag(line, i))
                    potentialOpeningTags.Add(i);
            }

            return potentialOpeningTags;
        }

        private List<int> GetPotentialClosingTag(string line)
        {
            var potentialClosingTags = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (IsStartOfClosingTag(line, i))
                    potentialClosingTags.Add(i);
            }

            return potentialClosingTags;
        }

        private bool IsStartOfOpeningTag(string line, int indexOfStart)
        {
            return IsMdTag(line, indexOfStart) &&
                   indexOfStart < line.Length - MdTag.Length &&
                   !char.IsWhiteSpace(line[indexOfStart + MdTag.Length]);
        }

        private bool IsStartOfClosingTag(string line, int indexOfStart)
        {
            return IsMdTag(line, indexOfStart) &&
                   indexOfStart > 0 &&
                   !char.IsWhiteSpace(line[indexOfStart - 1]);
        }

        private bool IsValidTag(string line, int start, int end)
        {
            return !textWorker.IsThereDigit(line, start, end) &&
                   !InsideTagEmptyString(start, end) &&
                   textWorker.ThereAreNotOnlySpecialChars(line, start, end) &&
                   (!textWorker.InTwoDifferentWords(line, start, end) ||
                    textWorker.IsStartOfWord(line, start) && textWorker.IsEndOfWord(line, end));
        }

        private bool InsideTagEmptyString(int openingTagStart, int closingTagStart)
        {
            return closingTagStart - (openingTagStart + MdTag.Length) == 0;
        }
    }
}