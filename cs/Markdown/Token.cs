using System;

namespace Markdown
{
    internal abstract class Token
    {
        public readonly int OpenIndex;

        public int Length { get; private set; } = -1;

        public bool IsOpened => Length == -1;

        protected Token(int openIndex)
        {
            OpenIndex = openIndex;
        }

        public void SetLength(int closeIndex)
        {
            if (closeIndex <= OpenIndex)
                throw new ArgumentException();

            if (Length != -1)
                throw new InvalidOperationException();

            Length = closeIndex - OpenIndex;
        }
    }
}