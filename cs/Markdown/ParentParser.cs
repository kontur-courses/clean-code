using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class ParentParser : IParser
    {
        protected IParser[] ChildParsers;
        public ParentParser(params IParser[] childParsers)
        {
            ChildParsers = childParsers;
        }

        public abstract ParsingResult Parse(string mdText, int startBoundary, int endBoundary);
        
        protected ParsingResult ParseChildren(TextType parentType ,string text, int startBoundary, int endBoundary)
        {
            var elements = new List<HyperTextElement>();
            var currentPlainTextStart = startBoundary;
            for (var index = startBoundary; index <= endBoundary; index++)
            {
                var result = ChildParsers.Select(parser => parser.Parse(text, index, endBoundary))
                    .FirstOrDefault(r => r.Success);
                if (result is null)
                    continue;
                if (currentPlainTextStart != index)
                    elements.Add(new HyperTextElement<string>(TextType.PlainText,
                        text.Substring(currentPlainTextStart, index - currentPlainTextStart)));
                elements.Add(result.Value);
                index = result.EndIndex;
                currentPlainTextStart = index + 1;
            }
            if (currentPlainTextStart <= endBoundary)
                elements.Add(new HyperTextElement<string>(TextType.PlainText,
                    text.Substring(currentPlainTextStart, endBoundary - currentPlainTextStart + 1)));
            if (elements.Count == 0)
                return ParsingResult.Fail();
            var tempElement = new HyperTextElement(parentType, elements.ToArray());
            return ParsingResult.Ok(tempElement, startBoundary, endBoundary);
        }
    }
}