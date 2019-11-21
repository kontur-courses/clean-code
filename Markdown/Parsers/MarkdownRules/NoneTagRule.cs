using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class NoneTagRule : IParserRule
    {
        public string OpenTag => "";
        public string CloseTag => "";
        public TagType TypeTag => TagType.NoneTag;
        public ParserNode FindFirstElement(string source, int startPosition = 0)
        {
            return null;
        }

        public bool CanUseInCurrent(TagType tagType) => false;
    }
}
