using System.Collections.Generic;

namespace Markdown.TokenParser
{
    public class BufferedEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;
        private readonly Queue<T> buffer = new();
        private int enumeratesCount;
        private T previous;

        public BufferedEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public bool MoveNext()
        {
            if (buffer.TryDequeue(out var peek))
            {
                if (buffer.Count > 0) previous = Current;
                Current = peek;
                return true;
            }

            if (enumerator.MoveNext())
            {
                previous = Current;
                enumeratesCount++;
                Current = enumerator.Current;
                return true;
            }

            return false;
        }

        public T Current { get; private set; }

        public void PushToBuffer(T next)
        {
            buffer.Enqueue(next);
        }

        public bool TryGetNext(out T next)
        {
            if (buffer.TryPeek(out next)) return true;
            if (enumerator.MoveNext())
            {
                buffer.Enqueue(enumerator.Current);
                next = enumerator.Current;
                return true;
            }

            return false;
        }

        public bool TryGetPrevious(out T value)
        {
            value = previous;
            return enumeratesCount > 1;
        }
    }
}