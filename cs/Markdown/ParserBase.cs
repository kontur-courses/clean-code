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
            var ignoredParsers = new HashSet<IParser>();
            for (var index = startBoundary; index <= endBoundary; index++)
            {
                ParsingResult result = null;
                foreach (var parser in ChildParsers.Where(parser => !ignoredParsers.Contains(parser)))
                {
                    result = parser.Parse(text, index, endBoundary);
                    if (result.Status == Status.NotFound)
                        continue;
                    if (result.Status == Status.Success)
                        break;
                    if (result.Status == Status.BadResult)
                        ignoredParsers.Add(parser);
                }
                if (result is not { Status: Status.Success })
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
                return ParsingResult.Fail(Status.NotFound);
            var tempElement = new HyperTextElement(parentType, elements.ToArray());
            return ParsingResult.Success(tempElement, startBoundary, endBoundary);
        }
    }
}