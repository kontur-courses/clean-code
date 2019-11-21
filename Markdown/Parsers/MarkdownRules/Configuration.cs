using System.Collections.Generic;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class Configuration
    {
        public static Dictionary<TagType, IParserRule> GetConfiguration()
        {
            var result = new Dictionary<TagType, IParserRule>();
            result[TagType.NoneTag] = new NoneTagRule();
            result[TagType.All] = new AllTagRule();

            result[TagType.Bold] = new BoldTagRule();
            result[TagType.Italic] = new ItalicTagRule();

            result[TagType.H1] = new HeaderTagType(1);
            result[TagType.H2] = new HeaderTagType(2);
            result[TagType.H3] = new HeaderTagType(3);
            result[TagType.H4] = new HeaderTagType(4);
            result[TagType.H5] = new HeaderTagType(5);
            result[TagType.H6] = new HeaderTagType(6);

            result[TagType.Paragraph] = new ParagraphTagRule();

            return result;
        }
    }
}
