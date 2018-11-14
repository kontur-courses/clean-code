using System;
using ConsoleApplication1.Interfaces;

namespace ConsoleApplication1.Parsers
{
    public class StringReader: IReader
    {
        private readonly string text;
        private int pointer;
        public StringReader(string text)
        {
            if (text == null)
                throw new ArgumentException("String reader can't get null instead of a string");
            this.text = text;
        }

        private void RaiseExceptionIfAllSymbolsAreWritten()
        {
            if (!AnySymbols())
                throw new InvalidOperationException("String reader does not contain new symbols");
        }

        public char ReadNextSymbol()
        {
            RaiseExceptionIfAllSymbolsAreWritten();
            return text[pointer++];
        }

        public bool AnySymbols()
            => pointer < text.Length;
    }
}
