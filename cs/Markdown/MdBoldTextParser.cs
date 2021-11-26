namespace Markdown
{
    public class MdBoldTextParser : MdQuotedParserBase
    {
        public static readonly MdBoldTextParser Default = new();

        private MdBoldTextParser() : base(TextType.BoldText, "__")
        {
            ChildParsers = new IParser[] { MdItalicTextParser.Default };
        }
    }
}