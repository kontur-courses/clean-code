using System;
using System.Collections.Generic;

namespace Markdown.Markups
{
    public abstract class Markup
    {
        public readonly string Opening;
        public readonly string Closing;
        private readonly string tag;
        private readonly List<Type> nestedMarkupTypes;

        protected Markup(string opening, string closing, string tag, List<Type> markupTypes)
        {
            Opening = opening;
            Closing = closing;
            this.tag = tag;
            nestedMarkupTypes = markupTypes;
        }

        public bool Contains(Type type)
        {
            return nestedMarkupTypes.Contains(type);
        }

        public string GetTaggedText(string text)
        {
            //return text != string.Empty ? $"<{tag}>{text}</{tag}>" : string.Empty;
            return $"<{tag}>{text}</{tag}>";
        }

        public RawToken GetRawToken(string text, int startPosition)
        {
            if (CanOpen(text, startPosition))
                for (var readPosition = startPosition + Opening.Length; readPosition < text.Length; readPosition++)
                    if (CanClose(text, readPosition))
                        return new RawToken(startPosition, readPosition, this);
            return null;
        }

        private bool CanOpen(string text, int openingPosition)
        {
            if (text.IsWhiteSpace(openingPosition + Opening.Length) || text.IsEscaped(openingPosition))
                return false;
            return text.ContainsAt(Opening, openingPosition);
        }

        private bool CanClose(string text, int closingPosition)
        {
            if (text.IsWhiteSpace(closingPosition - 1) || text.IsEscaped(closingPosition)
                                                       || !text.IsWhiteSpace(closingPosition + Closing.Length))

                return false;
            return text.ContainsAt(Closing, closingPosition);
        }
    }
}
