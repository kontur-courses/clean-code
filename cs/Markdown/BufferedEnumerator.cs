using System.Collections.Generic;

namespace Markdown
{
    public class BufferedEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;
        private readonly Queue<T> buffer = new();

        public BufferedEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public bool MoveNext()
        {
            if (buffer.TryDequeue(out var peek))
            {
                Current = peek;
                return true;
            }

            if (enumerator.MoveNext())
            {
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
    }
}