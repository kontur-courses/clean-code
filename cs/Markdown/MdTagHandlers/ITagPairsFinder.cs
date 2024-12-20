namespace Markdown.MdTagHandlers;

internal interface ITagPairsFinder
{
    public IEnumerable<TagReplacementsPair> PairTags(string str, IEnumerable<SubstringReplacement> tagPositions);

    public IEnumerable<TagReplacementsPair> PairFullLineTags(string str, IEnumerable<SubstringReplacement> tagPositions);
}
