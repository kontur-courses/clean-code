using System.Collections.Generic;
using System.Linq;

namespace Markdown.TagsLibrary
{
    public delegate bool SideSymbolsCheck(string leftSymbols, string rightSymbols);

    static partial class MarkdownTagsLibrary
    {
        public static readonly int MaximumTagLength;

        static MarkdownTagsLibrary()
        {
            MaximumTagLength = TagAssociations.Keys.Max().Length;
        }

        public static readonly IReadOnlyDictionary<string, (TagType type, int priority)> TagAssociations =
            new Dictionary<string, (TagType type, int priority)>
            {
                ["_"] = (TagType.Italics, 0),
                ["__"] = (TagType.Bold, 1),
            };

        public static readonly IReadOnlyDictionary<TagType, SideSymbolsCheck> StartTagRules =
            new Dictionary<TagType, SideSymbolsCheck>()
            {
                [TagType.Bold] = (left, right) => string.IsNullOrEmpty(right) || !right.StartsWith(" "),

                [TagType.Italics] = (left, right) =>
                {
                    if (string.IsNullOrEmpty(right))
                        return true;
                    if (!string.IsNullOrEmpty(left) && char.IsNumber(left.Last()) && char.IsNumber(right.First()))
                        return false;
                    return !right.StartsWith(" ");
                },
            };

        public static readonly IReadOnlyDictionary<TagType, SideSymbolsCheck> EndTagRules =
            new Dictionary<TagType, SideSymbolsCheck>()
            {
                [TagType.Bold] = (left, right) => string.IsNullOrEmpty(left) || !left.EndsWith(" "),

                [TagType.Italics] = (left, right) =>
                {
                    if (string.IsNullOrEmpty(left))
                        return true;
                    if (!string.IsNullOrEmpty(right) && char.IsNumber(right.First()) && char.IsNumber(left.Last()))
                        return false;
                    return !left.EndsWith(" ");
                },
            };

        public static readonly IReadOnlyDictionary<TagType, TagType[]> TagTypesToDeleteInRangeOtherTag =
            new Dictionary<TagType, TagType[]>()
            {
                [TagType.Bold] = new[] {TagType.None},
                [TagType.Italics] = new[] {TagType.Bold, TagType.None}
            };
    }
}