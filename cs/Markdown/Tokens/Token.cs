using System;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public abstract class Token

    {
        private bool isCorrect = true;
        public int OpenIndex { get; }
        public int CloseIndex { get; private set; }
        public bool IsOpened => CloseIndex == 0;
        public abstract bool IsNonPaired { get; }
        public abstract bool IsContented { get; }
        public virtual string AltText { get; set; }
        public virtual string Source { get; set; }

        public virtual bool IsCorrect
        {
            get => isCorrect;
            set
            {
                if (!isCorrect)
                    return;

                isCorrect = value;
            }
        }

        protected Token(int openIndex)
        {
            if (openIndex < 0)
                throw new ArgumentException("The open index must be greater than zero");

            OpenIndex = openIndex;
        }

        protected Token(int openIndex, int closeIndex)
        {
            if (openIndex < 0)
                throw new ArgumentException("The open index must be greater than zero");

            OpenIndex = openIndex;
            Close(closeIndex);
        }

        public void Close(int index)
        {
            if (!IsOpened)
                throw new InvalidOperationException("Token already closed");

            if (index < OpenIndex)
                throw new InvalidOperationException("The close index must be no larger than the open index");

            CloseIndex = index;
        }

        public abstract string GetSeparator();

        internal abstract bool Validate(IMdParser parser);

        public static bool IsCorrectTokenOpenIndex(int openIndex, string text, int length)
        {
            var indexNextToSeparator = openIndex + length;

            return openIndex != text.Length - 1 && indexNextToSeparator < text.Length &&
                   text[indexNextToSeparator] != ' ';
        }

        public static bool IsCorrectTokenCloseIndex(int closeIndex, string text)
        {
            return closeIndex != 0 && text[closeIndex - 1] != ' ';
        }
    }
}