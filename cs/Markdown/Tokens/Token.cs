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

        public abstract string GetSeparator();

        internal abstract void Accept(MdParser parser);

        public void Close(int index)
        {
            if (!IsOpened)
                throw new InvalidOperationException("Token already closed");

            if (index < OpenIndex)
                throw new InvalidOperationException("The close index must be no larger than the open index");

            CloseIndex = index;
        }
    }
}