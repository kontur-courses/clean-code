namespace Markdown
{
    public class MdQuotedParserBase : ParserBase
    {
        private TextType parsingType;
        public MdQuotedParserBase()
        {
            
        }
        
        public override ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            throw new System.NotImplementedException();
        }
    }
}