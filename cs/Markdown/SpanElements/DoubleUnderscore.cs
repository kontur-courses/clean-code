using System;
using System.Collections.Generic;

namespace Markdown
{
    public class DoubleUnderscore: ISpanElement
    {
        private const String indicator = "__";
        private readonly List<Type> possibleInnerSpanElements = new List<Type>()
        {
            typeof(SingleUnderscore)
        };

        public string GetOpeningIndicator()
        {
            return indicator;
        }

        public string GetClosingIndicator()
        {
            return indicator;
        }

        public string ToHtml(string markdown)
        {
            return $"<strong>{markdown}</strong>";
        }

        public bool Contains(ISpanElement spanElement)
        {
            return possibleInnerSpanElements.Contains(spanElement.GetType());
        }
    }
}
