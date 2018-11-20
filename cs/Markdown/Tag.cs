using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Tag
    {
        public LinkedListNode<string> InitialNode { get; private protected set; }
        public string End { get; private protected set; }
        public string Start { get; private protected set; }
        public Func<LinkedListNode<string>, bool> FindRule { get; private protected set; }
        public Func<LinkedListNode<string>, bool> CloseRule { get; private protected set; }
        private protected static Dictionary<string, Func<Tag>> RegisterTags = new Dictionary<string, Func<Tag>>();

        static Tag()
        {
            RegisterTags.Add("_", () => new TagEm());
            RegisterTags.Add("__", () => new TagStrong());
            RegisterTags.Add("[", () => new TagA());
        }

        public static bool TryOpenTag(LinkedListNode<string> currentNode, out Tag tag)
        {
            if (!RegisterTags.ContainsKey(currentNode.Value))
            {
                tag = null;
                return false;
            }

            var possibleTag = RegisterTags[currentNode.Value]();
            possibleTag.InitialNode = currentNode;

            if (!possibleTag.FindRule(currentNode))
            {
                tag = null;
                return false;
            }

            tag = possibleTag;
            return true;
        }

        public static bool TryCloseTag(LinkedListNode<string> currentNode, Tag lastOpenTag)
        {
            if (lastOpenTag.End != currentNode.Value || !lastOpenTag.CloseRule(currentNode)) return false;
            if (!(lastOpenTag is IPairTag tag)) return true;
            currentNode.Value = tag.EndTag;
            lastOpenTag.InitialNode.Value = tag.StartTag;
            return true;
        }
    }
}
