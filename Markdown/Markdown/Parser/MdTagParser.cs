using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Markdown.MdTag;
using NUnit.Framework.Constraints;

namespace Markdown.Parser
{
    class MdTagParser: IParser<Tag>
    {
        private readonly Dictionary<string, string> mdToHtmlMatches = new Dictionary<string, string>()
        {
            { "__", "strong" },
            { "_", "em" },
        };

        private readonly Dictionary<string, string> openAndCloseMdTags = new Dictionary<string, string>()
        {
            { "__", "__" },
            { "_", "_" },
        };

        private readonly Dictionary<string, List<string>> alowens = new Dictionary<string, List<string>>()
        {
            { "__", new List<string>{"__", "_"} },
            { "_", new List<string>{"_"} },
        };

        public List<Tag> Parse(string textToParse)
        {
            var tags = new List<Tag>();
            var tokens = textToParse.Split();
            var stack = new Stack<Tag>();
            foreach (var token in tokens)
            {
                var opendTag = GetTagFromStart(token);
                var closeTag = GetTagFromEnd(token);
                var isOpend = false;
                var isClosed = false;
                if (opendTag != token)
                {
                    var content = GetTagContent(opendTag.Length, closeTag.Length, token);
                    isOpend = OpenTag(opendTag, closeTag, stack, content, token);
                    isClosed = CloseTag(opendTag, closeTag, stack, content, tags, isOpend);
                }
                if (isClosed || isOpend) continue;
                AddTokenWithoutTag(stack, tags, token);
            }

            tags.AddRange(stack.Select(tag =>
            {
                var newTag = new Tag();
                newTag.tagContent.Add(tag.fullToken);
                return newTag;
            }));
            return tags;
        }

        private void AddTokenWithoutTag(Stack<Tag> stack, List<Tag> tags, string token)
        {
            if (stack.Count != 0)
            {
                stack.Peek().fullToken += " " + token;
                stack.Peek().tagContent[stack.Peek().tagContent.Count - 1] += " " + token;
            }
            else
            {
                var tag = new Tag();
                tag.tagContent.Add(token);
                tags.Add(tag);
            }
        }

        private bool CloseTag(string start, string end, Stack<Tag> stack, string context, List<Tag> tags, bool isOpend)
        {
            if (stack.Count == 0 || openAndCloseMdTags[stack.Peek().mdTag] != end) return false;
            var tag = stack.Pop();
            tag.fullToken = "";
            if (!isOpend)
                if (tag.tagContent.Count != 0)
                    tag.tagContent[tag.tagContent.Count - 1] += " " + context;
                else
                    tag.tagContent.Add(context);
            tag.htmlTag = mdToHtmlMatches[tag.mdTag];
            if (stack.Count == 0) tags.Add(tag);
            else stack.Peek().NestedTags.Add(tag);
            return true;
        }


        private bool OpenTag(string start, string end, Stack<Tag> stack, string context, string token)
        {
            if (stack.Count != 0 && alowens[stack.Peek().mdTag].Contains(start))
            {
                stack.Peek().tagContent.Add("");
                AddTagIntoStack(token, start, context, stack);
                return true;
            }
            if (stack.Count != 0 || !alowens.ContainsKey(start)) return false;
            AddTagIntoStack(token, start, context, stack);
            return true;
        }

        private void AddTagIntoStack(string token, string start, string context, Stack<Tag> stack)
        {
            var tag = new Tag { fullToken = token, mdTag = start };
            tag.tagContent.Add(context);
            stack.Push(tag);
        }

        private string GetTagContent(int ftl, int stl, string token) 
            => token.Substring(ftl, token.Length - ftl - stl); 

        private string GetTagFromStart(string mdString)
            => String.Join("", mdString.TakeWhile(symbol => !char.IsLetter(symbol)).ToList());

        private string GetTagFromEnd(string mdString) 
            => String.Join("", mdString.Reverse().TakeWhile(symbol => !char.IsLetter(symbol)).ToList());
    }
}
