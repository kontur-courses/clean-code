using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdParser : IParser
    {
        private readonly IParser[] childParsers;
        public static readonly MdParser Default = new MdParser();
        private MdParser()
        {
            childParsers = new IParser[] { MdParagraphParser.Default };
        }

        public ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            var elements = new List<HyperTextElement>();
            var index = startBoundary;
            while (index <= endBoundary)
            {
                var result = childParsers.Select(parser => parser.Parse(mdText, index, endBoundary))
                    .FirstOrDefault(r => r.IsSuccess);
                if (result is null)
                    break;
                elements.Add(result.Value);
                index = result.EndIndex + 1;
            }
            if (elements.Count == 0)
                return ParsingResult.Fail();
            var tempElement = new HyperTextElement(TextType.Body, elements.ToArray());
            return ParsingResult.Ok(tempElement, startBoundary, index);
        }

        public ParsingResult Parse(StringWithShielding mdText)
        {
            if (mdText.Length == 0)
                return ParsingResult.Ok(new HyperTextElement(TextType.Body), 0, 0);
            return Parse(mdText, 0, mdText.Length - 1);
        }
    }
}