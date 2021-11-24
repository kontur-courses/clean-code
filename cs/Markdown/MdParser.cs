using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdParser : IParser
    {
        private readonly IParser[] childParsers;
        public MdParser()
        {
            childParsers = new IParser[] { new MdParagraphParser() };
        }
        
        private  ParsingResult ParseChildren(string text, int startBoundary, int endBoundary)
        {
            var elements = new List<HyperTextElement>();
            var index = startBoundary;
            while (index <= endBoundary)
            {
                var result = childParsers.Select(parser => parser.Parse(text, index, endBoundary))
                    .FirstOrDefault(r => r.Success);
                if (result is null)
                    break;
                elements.Add(result.Value);
                index = result.EndIndex + 1;
            }
            if (elements.Count == 0)
                return ParsingResult.Fail();
            var tempElement = new HyperTextElement(TextType.None, elements.ToArray());
            return ParsingResult.Ok(tempElement, startBoundary, index);
        }
        
        public ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            var parsed = ParseChildren(mdText, startBoundary, endBoundary);
            parsed.Value.Type = TextType.Body;
            return parsed;
        }

        public ParsingResult Parse(string mdText)
        {
            return Parse(mdText, 0, mdText.Length - 1);
        }
    }
}