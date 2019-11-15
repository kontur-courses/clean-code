using System;
using System.Collections.Generic;
using MarkDown.TagParsers;

namespace MarkDown
{
    public class TraverseStack<T>
    {
        private class TraverseStackNode<T>
        {
            public TraverseStackNode<T> Previous;
            public TraverseStackNode<T> Next;
            public T Value;

            public TraverseStackNode(T value)
            {
                Value = value;
            }
        }

        private TraverseStackNode<T> first;
        private Dictionary<T, TraverseStackNode<T>> nodes;

        public TraverseStack()
        {
            nodes = new Dictionary<T, TraverseStackNode<T>>();
        }

        public void Push(T value)
        {
            if(nodes.ContainsKey(value))
                throw new ArgumentException("Traverse stack can't push equal elements!");
            if (first == null)
                first = new TraverseStackNode<T>(value);
            else
            {
                var newNode = new TraverseStackNode<T>(value);
                newNode.Next = first;
                first.Previous = newNode;
                first = newNode;
            }
            nodes[value] = first;
        }

        public T Pop()
        {
            if(first == null)
                throw new IndexOutOfRangeException();
            var popResult = first;
            nodes.Remove(popResult.Value);
            first = first.Next;
            return popResult.Value;
        }

        public bool Contains(T value)
        {
            return nodes.ContainsKey(value);
        }

        public IEnumerable<T> TraverseToElementAndReturnBack(T value)
        {
            if(!nodes.ContainsKey(value))
                throw new ArgumentException("No such element in stack!");
            var currentNode = first;
            while (!currentNode.Value.Equals(value))
            {
                yield return currentNode.Value;
                currentNode = currentNode.Next;
            }
            yield return currentNode.Value;
            currentNode = currentNode.Previous;
            while (currentNode != null)
            {
                yield return currentNode.Value;
                currentNode = currentNode.Previous;
            }
        }

        public void Remove(T value)
        {
            if(!nodes.ContainsKey(value))
                throw new ArgumentException("No such element in stack!");
            var nodeToRemove = nodes[value];
            if (nodeToRemove == first)
                Pop();
            else if(nodeToRemove.Next == null)
            {
                nodeToRemove.Previous.Next = null;
                nodes.Remove(value);
            }
            else
            {
                nodeToRemove.Previous.Next = nodeToRemove.Next;
                nodeToRemove.Next.Previous = nodeToRemove.Previous;
                nodes.Remove(value);
            }
        }
    }
}