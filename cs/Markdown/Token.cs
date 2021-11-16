using System;

namespace Markdown
{
    public abstract class Token
    {
        public int OpenIndex { get; }

        public int CloseIndex { get; protected set; }

        public bool IsOpened => CloseIndex == 0;

        public static string Separator;

        protected Token(int openIndex)
        {
            if (OpenIndex < 0)
                throw new ArgumentException();

            OpenIndex = openIndex;
        }

        internal abstract void Handle(MdParser parser);
    }
}