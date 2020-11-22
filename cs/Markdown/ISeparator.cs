using System.Collections.Generic;

namespace Markdown
{
    public interface ISeparator
    {
        int Position { get; }
        string Value { get; }
        string Text { get; }

        bool IsItCorrectOpeningSeparator();
        bool IsItCorrectClosingSeparator();
        bool IsSeparatorsInteractionCorrect(ISeparator openingSeparator, Stack<ISeparator> separators);
    }
}