using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class ParserBase : IParser
    {
        protected IParser[] ChildParsers;
        
        protected ParserBase(params IParser[] childParsers)
        {
            ChildParsers = childParsers;
        }

        public abstract ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary);
        
        protected ParsingResult ParseChildren(TextType parentType ,StringWithShielding text, int startBoundary, int endBoundary)
        {
            var elements = new List<HyperTextElement>();
            var currentPlainTextStart = startBoundary;
            for (var index = startBoundary; index <= endBoundary; index++)
            {
                var result = ChildParsers.Select(parser => parser.Parse(text, index, endBoundary))
                    .FirstOrDefault(r => r.IsSuccess);
                if (result is null)
                    continue;
                if (currentPlainTextStart != index)
                    elements.Add(new HyperTextElement<string>(TextType.PlainText,
                        text.ShieldedSubstring(currentPlainTextStart, index - 1)));
                elements.Add(result.Value);
                index = result.EndIndex;
                currentPlainTextStart = index + 1;
            }
            if (currentPlainTextStart <= endBoundary)
                elements.Add(new HyperTextElement<string>(TextType.PlainText,
                    text.ShieldedSubstring(currentPlainTextStart, endBoundary)));
            if (elements.Count == 0)
                return ParsingResult.Fail();
            var tempElement = new HyperTextElement(parentType, elements.ToArray());
            return ParsingResult.Ok(tempElement, startBoundary, endBoundary);
        }
    }
}