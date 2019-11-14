using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class QueueItem<T>
    {
        public readonly T Value;
        public QueueItem<T> Next;

        public QueueItem(T value, QueueItem<T> next)
        {
            this.Value = value;
            this.Next = next;
        }
    }

    class FixedSizeQueue<T>
    {
        private readonly int Size;
        QueueItem<T> head;
        QueueItem<T> tail;
        int Count;

        public FixedSizeQueue(int size)
        {
            head = null;
            tail = null;
            this.Size = size;
        }

        public bool IsEmpty { get { return head == null; } }

        public void Enqueue(T value)
        {
            if (IsEmpty)
                tail = head = new QueueItem<T>(value, null);
            else
            {
                var item = new QueueItem<T>(value, null);
                tail.Next = item;
                tail = item;
            }
            Count++;
            while (Count > Size)
                this.Dequeue();
        }

        public T Dequeue()
        {
            if (head == null) throw new InvalidOperationException();
            var result = head.Value;
            head = head.Next;
            if (head == null)
                tail = null;
            Count--;
            return result;
        }

        public T GetHead()
        {
            return head.Value;
        }

        public T Get(int index)
        {
            var outIndex = 0;
            QueueItem<T> outItem = head;
            while (outIndex < index)
            {
                outItem = outItem.Next;
                outIndex++;
            }
            return outItem.Value;
        }

        public T Last()
        {
            return tail==null ? default : tail.Value;
        }
    }
}
