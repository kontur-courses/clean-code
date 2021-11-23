using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class EscapedTextBuilder : IEnumerable<Context>
    {
        private readonly TextIterator textIterator;
        private int escapedCount;
        private readonly StringBuilder builder = new();
        private readonly List<int> escapedSymbolsBefore = new();

        public EscapedTextBuilder(Context context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            textIterator = new TextIterator(context);
        }

        public void SkipNextSymbol()
        {
            escapedCount += 2;
            textIterator.SkipNext();
        }

        public void Append(char symbol)
        {
            builder.Append(symbol);
            escapedSymbolsBefore.Add(escapedCount);
        }

        public EscapedText Build() => new(builder.ToString(), escapedSymbolsBefore);
        public IEnumerator<Context> GetEnumerator() => textIterator.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}