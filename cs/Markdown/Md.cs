using Markdown.MdTagHandlers;
using Markdown.MdTags;
using Markdown.MdTags.Interfaces;
using System.Text;

namespace Markdown;

public class Md : IStringProcessor
{
    private readonly ITagPairsFinder tagPairsFinder;
    private readonly IPairTagsIntersectionHandler pairTagsIntersectionHandler;
    private readonly IGroupTagInsertionsFinder groupTagInsertionsFinder;
    private readonly IMdTag[] mdTags;

    public Md() : this(
        new TagPairsFinder(),
        new PairTagsIntersectionHandler(),
        new GroupTagInsertionsFinder(),
        [
            new UnderscoresBetweenNumbersIgnoreTag(),
            new EscapeMdTag(),
            new OrderedListItemMdTag(),
            new UnorderedListItemMdTag(),
            new HeaderMdTag(),
            new ImageMdTag(),
            new LinkMdTag(),
            new BoldTextMdTag(),
            new ItalicTextMdTag(),
        ])
    {
    }

    internal Md(
        ITagPairsFinder tagPairsFinder,
        IPairTagsIntersectionHandler pairTagsIntersectionHandler,
        IGroupTagInsertionsFinder groupTagInsertionsFinder,
        params IMdTag[] tags)
    {
        ArgumentNullException.ThrowIfNull(tagPairsFinder);
        ArgumentNullException.ThrowIfNull(pairTagsIntersectionHandler);
        ArgumentNullException.ThrowIfNull(groupTagInsertionsFinder);

        this.tagPairsFinder = tagPairsFinder;
        this.pairTagsIntersectionHandler = pairTagsIntersectionHandler;
        this.groupTagInsertionsFinder = groupTagInsertionsFinder;

        tags ??= [];

        foreach (var tag in tags)
        {
            ArgumentNullException.ThrowIfNull(tag);

            if (!IsTagWithSupportedType(tag))
            {
                throw new ArgumentException($"Tag '{tag.GetType()}' is unsupported.");
            }
        }

        mdTags = tags;
    }

    public string Render(string mdString)
    {
        ArgumentNullException.ThrowIfNull(mdString);

        var tagsIndices = GetTagsIndices(mdString, mdTags)
            .GroupBy(kv => kv.Key.TagType)
            .ToDictionary(group => group.Key, group => group.ToDictionary());
        var htmlReplacements = new Dictionary<int, SubstringReplacement>();

        if (tagsIndices.TryGetValue(TagType.Normal, out var normalTagsIndices))
        {
            AddReplacementsRange(htmlReplacements, GetNormalTagsReplacements(mdString, normalTagsIndices));
        }

        if (tagsIndices.TryGetValue(TagType.Pair, out var pairTagsIndices))
        {
            AddReplacementsRange(htmlReplacements, GetPairTagsReplacements(mdString, pairTagsIndices));
        }

        if (tagsIndices.TryGetValue(TagType.FullLine, out var fullLineTagsIndices))
        {
            AddReplacementsRange(htmlReplacements, GetFullLineTagsReplacements(mdString, fullLineTagsIndices));
        }

        return GetHtmlString(mdString, htmlReplacements);
    }

    private bool IsTagWithSupportedType(IMdTag tag)
    {
        return tag.TagType switch
        {
            TagType.Normal => tag is IHtmlConverter and not IGroupedMdTag,
            TagType.Pair => tag is IHtmlTagsPair,
            TagType.FullLine => tag is IHtmlTagsPair,
            TagType.Ignore => true,
            _ => false,
        };
    }

    private Dictionary<IMdTag, List<SubstringReplacement>> GetTagsIndices(string mdString, IEnumerable<IMdTag> tags)
    {
        var tagsIndices = tags.ToDictionary(tag => tag, _ => new List<SubstringReplacement>());

        for (var i = 0; i < mdString.Length; i++)
        {
            foreach (var tag in tags)
            {
                if (tag.IsMdTag(mdString, i, out var tagLength))
                {
                    if (tag.TagType != TagType.Ignore)
                    {
                        tagsIndices[tag].Add(new SubstringReplacement(i, tagLength));
                    }

                    i += tagLength - 1;
                    break;
                }
            }
        }

        return tagsIndices;
    }

    private void AddReplacementsRange(
        Dictionary<int, SubstringReplacement> replacements,
        IEnumerable<SubstringReplacement> replacementsToAdd)
    {
        foreach (var newReplacement in replacementsToAdd)
        {
            if (replacements.TryGetValue(newReplacement.Index, out var replacement))
            {
                var replacementsConcat = replacement.Replacement!.Contains('/')
                    ? replacement.Replacement + newReplacement.Replacement
                    : newReplacement.Replacement + replacement.Replacement;
                replacement = replacement with { Replacement = replacementsConcat };
            }
            else
            {
                replacement = newReplacement;
            }

            replacements[newReplacement.Index] = replacement;
        }
    }

    private IEnumerable<SubstringReplacement> GetNormalTagsReplacements(
        string mdString,
        Dictionary<IMdTag, List<SubstringReplacement>> normalTagsPositions)
    {
        return normalTagsPositions
            .SelectMany(kv => kv.Value
                .Select(x => x with { Replacement = ((IHtmlConverter)kv.Key).GetHtmlString(mdString, x.Index) }));
    }

    private IEnumerable<SubstringReplacement> GetPairTagsReplacements(
        string mdString,
        Dictionary<IMdTag, List<SubstringReplacement>> pairTagsPositions)
    {
        var pairedTags = pairTagsPositions
            .ToDictionary(kv => kv.Key, kv => tagPairsFinder.PairTags(mdString, kv.Value).ToList());
        var tagPairs = pairTagsIntersectionHandler
            .RemoveIntersections(pairedTags)
            .ToDictionary(kv => (IHtmlTagsPair)kv.Key, kv => kv.Value.AsEnumerable());

        return GetPairedTagsReplacements(mdString, tagPairs);
    }

    private IEnumerable<SubstringReplacement> GetFullLineTagsReplacements(
        string mdString,
        Dictionary<IMdTag, List<SubstringReplacement>> fullLineTagsPositions)
    {
        var pairedTags = fullLineTagsPositions
            .ToDictionary(kv => (IHtmlTagsPair)kv.Key, kv => tagPairsFinder.PairFullLineTags(mdString, kv.Value));

        return GetPairedTagsReplacements(mdString, pairedTags);
    }

    private IEnumerable<SubstringReplacement> GetPairedTagsReplacements(
        string mdString,
        Dictionary<IHtmlTagsPair, IEnumerable<TagReplacementsPair>> pairedTags)
    {
        var htmlReplacements = pairedTags
            .SelectMany(kv => kv.Value
                .SelectMany(pair => pair.GetHtmlReplacements(kv.Key).Flatten()));
        var groupTagsHtmlInsertions = pairedTags
            .Where(kv => kv.Key is IGroupedMdTag)
            .SelectMany(kv => groupTagInsertionsFinder
                .GetGroupTagReplacements(mdString, ((IGroupedMdTag)kv.Key).GroupHtmlTagsPair, kv.Value));

        return htmlReplacements
            .Concat(groupTagsHtmlInsertions);
    }

    private string GetHtmlString(string mdString, Dictionary<int, SubstringReplacement> htmlReplacements)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < mdString.Length; i++)
        {
            var hasReplacement = htmlReplacements.TryGetValue(i, out var replacement);

            if (hasReplacement)
            {
                sb.Append(replacement.Replacement);
                i += Math.Max(0, replacement.Length - 1);
            }

            if (!hasReplacement || hasReplacement && replacement.Length == 0)
            {
                sb.Append(mdString[i]);
            }
        }

        if (htmlReplacements.TryGetValue(mdString.Length, out var lastReplacement))
        {
            sb.Append(lastReplacement.Replacement);
        }

        return sb.ToString();
    }
}
