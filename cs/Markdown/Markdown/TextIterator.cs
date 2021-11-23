using System;
using System.Collections;
using System.Collections.Generic;

namespace Markdown
{
    public class TextIterator : IEnumerable<Context>, IEnumerator<Context>
    {
        public Context Current { get; private set; }
        object IEnumerator.Current => Current;

        private readonly Context context;
        private readonly int startIndex;

        public TextIterator(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            startIndex = context.Index;
        }

        public void SkipNext() => context.Index += 1;

        public IEnumerator<Context> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            if (context.Index >= context.Text.Length - 1)
                return false;

            if (Current == null)
                Current = context;
            else
                context.Index++;

            return true;
        }

        public void Reset()
        {
            context.Index = startIndex;
            Current = null;
        }

        public void Dispose() => Reset();
    }
}