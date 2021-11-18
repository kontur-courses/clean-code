using System;
using System.Collections.Generic;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public abstract class Token
    {
        public static readonly Dictionary<Type, string> Separators = new()
        {
            { typeof(HeaderToken), "#" },
            { typeof(ItalicToken), "_" },
            { typeof(BoldToken), "__" },
            { typeof(ScreeningToken), "\\" }
        };

        public int OpenIndex { get; }

        public int CloseIndex { get; private set; }

        public bool IsOpened => CloseIndex == 0;

        public string Separator => Separators[GetType()];

        protected Token(int openIndex)
        {
            ValidateOpenIndex(openIndex);

            OpenIndex = openIndex;
        }

        protected Token(int openIndex, int closeIndex)
        {
            ValidateOpenIndex(openIndex);
            OpenIndex = openIndex;
            Close(closeIndex);
        }

        private static void ValidateOpenIndex(int openIndex)
        {
            if (openIndex < 0)
                throw new ArgumentException("The open index must be greater than zero");
        }

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