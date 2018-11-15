using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace Markdown
{
    public class SpecialLinkedList<Type> : IEnumerable<Type>
    {
        private Node<Type> head;

        public SpecialLinkedList(IEnumerable<Type> collection)
        {
            head = new Node<Type>();
            var current = head;
            foreach (var element in collection)
            {
                current.value = element;
                current.next = new Node<Type>();
                current = current.next;
            }
        }
        public void PushFront(Type value)
        {
            var temp = new Node<Type>();
            temp.value = value;
            temp.next = head;
            head = temp;
        }

        public Type Take()
        {
            var ret = head.value;
            head = head.next;
            return ret;
        }
        public IEnumerator<Type> GetEnumerator()
        {
            return this.EnumerateCollections().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerable<Type> EnumerateCollections()
        {
            while (head != null)
            {
                var ret = head.value;
                head = head.next;
                yield return ret;
            }
        }
            
    }

    public class Node<Type>
    {
        public Type value;
        public Node<Type> next;
        
    }

    public static class StringExtensions
    {
        public static SpecialLinkedList<char> ToSpecialLinkedList(this string str)
        {
            return new SpecialLinkedList<char>(str);
        }
    }
}