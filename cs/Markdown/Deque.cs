using System;
using System.Collections;
using System.Collections.Generic;

namespace Markdown
{
    public class Deque<T> : IEnumerable<T> 
    {
        private DoublyNode<T> head; 
        private DoublyNode<T> tail;

        public void AddLast(T data)
        {
           var node = new DoublyNode<T>(data);
 
            if (head == null)
                head = node;
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;
            Count++;
        }
        public void AddFirst(T data)
        {
            var node = new DoublyNode<T>(data);
            var temp = head;
            node.Next = temp;
            head = node;
            if (Count == 0)
                tail = head;
            else
                temp.Previous = node;
            Count++;
        }
        public T RemoveFirst()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            var output = head.Data;
            if(Count==1)
            {
                head = tail = null;
            }
            else
            {
                head = head.Next;
                head.Previous = null;
            }
            Count--;
            return output;
        }
        public T RemoveLast()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            var output = tail.Data;
            if (Count == 1)
            {
                head = tail = null;
            }
            else
            {
                tail = tail.Previous;
                tail.Next = null;
            }
            Count--;
            return output;
        }
        public T First
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException();
                return head.Data;
            }
        }
        public T Last
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException();
                return tail.Data;
            }
        }
 
        public int Count { get; private set; }
        public bool IsEmpty => Count == 0;

        public void Clear()
        {
            head = null;
            tail = null;
            Count = 0;
        }
 
        public bool Contains(T data)
        {
            var current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }
 
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
 
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
    
    public class DoublyNode<T>
    {
        public DoublyNode(T data)
        {
            Data = data;
        }
        public T Data { get; private set; }
        public DoublyNode<T> Previous { get; set; }
        public DoublyNode<T> Next { get; set; }
    }
}