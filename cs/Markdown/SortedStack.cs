using System;
using System.Collections.Generic;

namespace Markdown
{
    public class SortedStack<TValue>
        where TValue : IComparable
    {
        private readonly SortedSet<TValue> set;
        public int Count => set.Count;
        
        public SortedStack()
        {
            set = new SortedSet<TValue>();
        }
        
        public void Push(TValue value)
        {
            set.Add(value);
        }
        
        public TValue Peek()
        {
            if (set.Count == 0)
                throw new IndexOutOfRangeException("Stack is empty"); 
            return set.Min;
        }
        
        public TValue Pop()
        {
            if (set.Count == 0)
                throw new IndexOutOfRangeException("Stack is empty");
            var minValue = set.Min;
            set.Remove(minValue);
            return minValue;
        }
    }
}