using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdParagraphParser : ParserBase
    {
        public static readonly MdParagraphParser Default = new MdParagraphParser();
        private MdParagraphParser()
        {
            ChildParsers = new IParser[] { MdLinkAndImageParser.Instance,  MdBoldTextParser.Default, MdItalicTextParser.Default};
        }

        public override ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            var type = TextType.Paragraph;
            var childrenStart = startBoundary;
            if (mdText.ContainsAt(startBoundary, Md.HeaderSymbol + " "))
            {
                type = TextType.Header;
                childrenStart += 2;
            }
            var (paragraphEnd, childrenEnd) = FindParagraphEnd(mdText, startBoundary, endBoundary);
            var children = ParseChildren(type, mdText, childrenStart, childrenEnd);
            return children.Status != Status.Success ? children : ParsingResult.Success(children.Value, startBoundary, paragraphEnd);
        }

        private static (int paragraphEnd, int childBlockEnd)  FindParagraphEnd(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            var paragraphEnd = mdText.IndexOf('\n', startBoundary);
            if (paragraphEnd < 0 || paragraphEnd > endBoundary)
                paragraphEnd = endBoundary + 1;
            var childrenEnd = paragraphEnd - 1;
            if (mdText[childrenEnd] == '\r')
                childrenEnd--;
            return (paragraphEnd, childrenEnd);
        }
    }
}