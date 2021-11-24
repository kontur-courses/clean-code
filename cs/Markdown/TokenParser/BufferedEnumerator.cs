using System.Collections.Generic;

namespace Markdown.TokenParser
{
    public class BufferedEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;
        private readonly Queue<T> buffer = new();
        private int count;
        private T previous;
        private T current;

        public BufferedEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public bool MoveNext()
        {
            if (buffer.TryDequeue(out var peek))
            {
                if (buffer.Count > 0) previous = current;
                current = peek;
                return true;
            }

            if (enumerator.MoveNext())
            {
                previous = Current;
                Current = enumerator.Current;
                return true;
            }

            return false;
        }

        public T Current
        {
            get => current;
            private set
            {
                count++;
                current = value;
            }
        }

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
            return count > 1;
        }
    }
}