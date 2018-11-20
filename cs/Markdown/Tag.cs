using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Markdown
{
    public abstract class Tag
    {
        public TextSeparator StartSeparator { get; private set; }
        public LinkedListNode<string> nodeFun { get; private protected set; }
        public string End { get; private protected set; }
        public string Start { get; private protected set; }
        public Func<LinkedListNode<string>, bool> FindRule { get; private protected set; }
        public Func<LinkedListNode<string>, bool> CloseRule { get; private protected set; }
        private protected static Dictionary<string, Func<Tag>> RegisterTags = new Dictionary<string, Func<Tag>>();
        private protected static Dictionary<string, Func<Tag, bool>> OperationTags = new Dictionary<string, Func<Tag, bool>>();

        static Tag()
        {
            Type baseType = typeof(Tag);           
            // Непосредственные наследники
            var derivedTypes = baseType.Assembly.ExportedTypes.Where(t => t.BaseType == baseType);            
            // todo get Teg children (container)
            //derivedTypes.GetEnumerator()
            RegisterTags.Add("_", () => new TagEm());
            RegisterTags.Add("__", () => new TagStrong());
            RegisterTags.Add("[", () => new TagA());

            OperationTags.Add("]", (t) =>
            {
                if (!(t is TagA)) return false;
                t.Start = "asf";
                return true;
            });
        }

        public static bool FindMyPapapap(LinkedListNode<string> currentNode, out Tag tag)
        {
            Tag ptag;
            if (!RegisterTags.ContainsKey(currentNode.Value))
            {
                tag = null; // :c
                return false;
            }

            ptag = RegisterTags[currentNode.Value]();
            ptag.nodeFun = currentNode;

            if (!ptag.FindRule(currentNode))
            {
                tag = null; // :c
                return false;
            }

            tag = ptag;
            return true;
        }

        public static bool rkarak(LinkedListNode<string> currentNode, Tag lastOpenTag)
        {
            if (lastOpenTag.End == currentNode.Value)
                return lastOpenTag.CloseRule(currentNode);
            return false;
        }

        public static Tag CreateTagOnTextSeparator(TextSeparator textSeparator)
        {
            var tag = RegisterTags[textSeparator.Separator]();
            tag.StartSeparator = textSeparator;
            return tag;
        }
    }
}
