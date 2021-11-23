namespace Markdown
{
    internal interface ITokenParser
    {
        public (SegmentsCollection, SegmentsCollection) IgnoreSegmentsThatDoNotMatchRules(SegmentsCollection first, SegmentsCollection second);

        string ReplaceTokens(string text, SegmentsCollection segments, ITagTranslator translator);

        TokenInfoCollection FindAllTokens(string paragraph);
    }
}