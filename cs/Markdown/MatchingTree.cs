using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;

namespace Markdown
{
    public class MatchingTree<Type>
    {
        private MathcingNode<Type> head;
        private HashSet<MathcingNode<Type>> tagedNodes;
        
        public MatchingTree(IEnumerable<IEnumerable<Type>> collection)
        {
            tagedNodes = new HashSet<MathcingNode<Type>>();
            head = new MathcingNode<Type>();
            foreach (var element in collection)
            {
                var current = head;
                foreach (var subElement in element)
                {
                    if(current.GetNode(subElement) == null)
                        current.AddNewContinuation(subElement);
                    current = current.GetNode(subElement);
                }
                tagedNodes.Add(current);
            }
        }

        public bool IsTaged(MathcingNode<Type> node)
        {
            return tagedNodes.Contains(node);
        }

        public MathcingNode<Type> GetHead()
        {
            return head;
        }
    }

    public class MathcingNode<Type>
    {
        private Dictionary<Type,MathcingNode<Type>> dict;

        public MathcingNode()
        {
            dict = new Dictionary<Type, MathcingNode<Type>>();
        }

        public void AddNewContinuation(Type token, MathcingNode<Type> node = null)
        {
            if(node == null)
                dict.Add(token,new MathcingNode<Type>());
            else
                dict.Add(token,node);
        }

        public MathcingNode<Type> GetNode(Type token)
        {
            return dict.ContainsKey(token) ? dict[token] : null;
        }
    }

}