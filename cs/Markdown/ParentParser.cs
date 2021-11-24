using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class ParentParser : IParser
    {
        protected IParser[] childParsers;

        public ParentParser(params IParser[] childParsers)
        {
            childParsers = childParsers;
        }

        protected ParsingResult ParseChildren(string text, int startBoundary, int endBoundary)
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
            var tempElement = new HyperTextElement(null, elements.ToArray());
            return ParsingResult.Ok(tempElement, startBoundary, index);
        }

        public abstract ParsingResult Parse(string mdText, int startBoundary, int endBoundary);
    }
}