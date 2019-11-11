using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Markdown
{
    public static class LinkedListExtensions
    {
        public static void RemoveMany<T>(this LinkedList<T> linkedList, LinkedListNode<T> node, int n)
        {
            var currentNode = node;
            for (var _ = 0; _ < n && currentNode != null; ++_)
            {
                linkedList.Remove(currentNode);
                currentNode = currentNode.Next;
            }
        }

        public static void RemoveRange<T>(this LinkedList<T> linkedList, LinkedListNode<T> firstNode,
            LinkedListNode<T> lastNode)
        {
            var currentNode = firstNode;
            do
            {
                if (currentNode != null)
                {
                    var nextNode = currentNode.Next;
                    linkedList.Remove(currentNode);
                    currentNode = nextNode;
                }
                else
                {
                    throw new ArgumentException("firstNode should go before lastNode in LinkedList");
                }
            } while (currentNode != lastNode);

            linkedList.Remove(lastNode ?? throw new ArgumentNullException(nameof(lastNode)));
        }

        public static List<T> GetAsList<T>(this LinkedList<T> linkedList, LinkedListNode<T> firstNode,
            LinkedListNode<T> lastNode)
        {
            var list = new List<T>();
            var currentNode = firstNode;
            do
            {
                if (currentNode != null)
                {
                    list.Add(currentNode.Value);
                    currentNode = currentNode.Next;
                }
                else
                {
                    throw new ArgumentException("firstNode should go before lastNode in LinkedList");
                }
            } while (currentNode != lastNode);

            return list;
        }

        public static LinkedListNode<T> GetNthAfter<T>(this LinkedListNode<T> linkedListNode, int n)
        {
            var currentNode = linkedListNode;
            for (var _ = 0; _ < n && linkedListNode.Next != null; ++_)
            {
                linkedListNode = linkedListNode.Next;
            }

            return currentNode;
        }
    }
}