using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class BoldSeparator : ItalicSeparator, ISeparator
    {
        public BoldSeparator(int position, string text) : base(position, text)
        {
            Value = "__";
        }

        bool ISeparator.IsSeparatorsInteractionCorrect(ISeparator openingSeparator, Stack<ISeparator> separators)
        {
            var boldIsInsideItalic = separators.Any() && separators.Peek() is ItalicSeparator;
            return !boldIsInsideItalic && !AreSeparatorsInsideDifferentWords((BoldSeparator) openingSeparator);
        }
    }
}