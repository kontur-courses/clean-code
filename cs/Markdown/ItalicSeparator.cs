using System.Collections.Generic;

namespace Markdown
{
    internal class ItalicSeparator : ISeparator
    {
        public ItalicSeparator(int position, string text)
        {
            Position = position;
            Text = text;
            Value = "_";
        }

        public int Position { get; }
        public string Value { get; protected set; }
        public string Text { get; }

        public bool IsItCorrectOpeningSeparator()
        {
            var nextPosition = Position + Value.Length;
            return nextPosition < Text.Length &&
                   Text[nextPosition] != ' ' && !IsSeparatorInsideTextWithDigits();
        }

        public bool IsItCorrectClosingSeparator()
        {
            return Text[Position - 1] != ' ' && !IsSeparatorInsideTextWithDigits();
            ;
        }

        bool ISeparator.IsSeparatorsInteractionCorrect(ISeparator openingSeparator, Stack<ISeparator> separators)
        {
            return !AreSeparatorsInsideDifferentWords((ItalicSeparator) openingSeparator);
        }

        private bool IsSeparatorInsideTextWithDigits()
        {
            var previousPosition = Position - 1;
            var nextPosition = Position + Value.Length;
            return previousPosition > 0 &&
                   (char.IsDigit(Text[previousPosition]) || char.IsLetter(Text[previousPosition])) &&
                   nextPosition < Text.Length &&
                   (char.IsDigit(Text[nextPosition]) || char.IsLetter(Text[nextPosition])) &&
                   (char.IsDigit(Text[nextPosition]) || char.IsDigit(Text[previousPosition]));
        }

        private bool IsSeparatorInsideWord()
        {
            return Position > 0 && char.IsLetter(Text[Position - 1]) &&
                   Position + Value.Length < Text.Length &&
                   char.IsLetter(Text[Position + Value.Length]);
        }

        protected bool AreSeparatorsInsideDifferentWords(ItalicSeparator openingSeparator)
        {
            var textBetweenSeparators = Text.Substring(openingSeparator.Position,
                Position - openingSeparator.Position);
            return openingSeparator.IsSeparatorInsideWord() && IsSeparatorInsideWord() &&
                   textBetweenSeparators.Contains(" ");
        }
    }
}