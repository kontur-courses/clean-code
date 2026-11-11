using Markdown.Entity;
using Markdown.Enums;
using Markdown.Parser.Inline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Block
{
    public static class ParagraphParser
    {
        public static Paragraph CreateParagraph(string paragraphText)
        {
            var currentParagraph = new Paragraph(GetParagraphType(paragraphText));

            var nodeTree = InlineParser.GetTagsTree(
                currentParagraph.Type == ParagraphType.Heading ? paragraphText.Substring(2) : paragraphText);

            currentParagraph.SetTagTree(nodeTree);
            return currentParagraph;
        }

        private static ParagraphType GetParagraphType(string text) 
        {
            if (!string.IsNullOrEmpty(text) && text.StartsWith("# ")) 
                return ParagraphType.Heading;
            return ParagraphType.Paragraph;
        }
    }
}
