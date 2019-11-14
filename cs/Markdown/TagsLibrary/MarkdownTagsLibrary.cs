using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public delegate bool SideSymbolsCheck(char? leftSymbol, char? rightSymbol);

    static partial class MarkdownTagsLibrary
    {
        static MarkdownTagsLibrary()
        {
            MaximumTagLength = TagAssociations.Keys.Max().Length;
        }

        public static readonly int MaximumTagLength;

        public static readonly IReadOnlyDictionary<string, (TagType type, int priority)> TagAssociations =
            new Dictionary<string, (TagType type, int priority)>
            {
                ["_"] = (TagType.Italics, 0),
                ["__"] = (TagType.Bold, 1),
            };

        public static readonly IReadOnlyDictionary<TagType, SideSymbolsCheck> StartTagRules =
            new Dictionary<TagType, SideSymbolsCheck>()
            {
                [TagType.Bold] = (left, right) => !right.HasValue || right != ' ',

                [TagType.Italics] = (left, right) =>
                {
                    if (!right.HasValue)
                        return true;
                    if (left.HasValue && char.IsNumber(left.Value) && char.IsNumber(right.Value))
                        return false;
                    return right != ' ';
                },
            };

        public static readonly IReadOnlyDictionary<TagType, SideSymbolsCheck> EndTagRules =
            new Dictionary<TagType, SideSymbolsCheck>()
            {
                [TagType.Bold] = (left, right) => !left.HasValue || left != ' ',

                [TagType.Italics] = (left, right) =>
                {
                    if (!left.HasValue)
                        return true;
                    if (right.HasValue && char.IsNumber(left.Value) && char.IsNumber(right.Value))
                        return false;
                    return left != ' ';
                },
            };

        public static readonly IReadOnlyDictionary<TagType, (string start, string end)> TagToHtmlTagAssociations =
            new Dictionary<TagType, (string start, string end)>()
            {
                [TagType.Bold] = ("<strong>", "</strong>"),
                [TagType.Italics] =  ("<em>", "</em>"),
            };

        public static readonly IReadOnlyDictionary<TagType, TagType[]> TagTypesToDeleteInRangeOtherTag =
            new Dictionary<TagType, TagType[]>()
            {
                [TagType.Bold] = new[] {TagType.None},
                [TagType.Italics] = new[] {TagType.Bold, TagType.None}
            };
    }
}