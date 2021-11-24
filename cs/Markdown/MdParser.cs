using System;

namespace Markdown
{
    public class MdParser : ParentParser
    {
        public MdParser()
        {
            childParsers = new IParser[] { new MdHeaderParser(), new MdParagraphParser() };
        }
        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            var parsed = ParseChildren(mdText, startBoundary, endBoundary);
            parsed.Value.Type = "Body";
            return parsed;
        }

        public ParsingResult Parse(string mdText)
        {
            return Parse(mdText, 0, mdText.Length - 1);
        }
    }
}