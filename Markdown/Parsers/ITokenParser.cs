namespace Markdown
{
    internal interface ITokenParser
    {
        public TagRules GetRules();

        string ReplaceTokens(string text, SegmentsCollection segments, ITagTranslator translator);

        TokenInfoCollection FindAllTokens(string paragraph);
    }
}