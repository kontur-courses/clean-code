using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework.Constraints;

namespace Markdown
{
    class Md
    {
        public string Render(string mdString)
        {
            var elements = FindElements(mdString);
            return CreateHtml(mdString, elements);
        }

        public string CreateHtml(string mdString, List<ElementData> elements)
        {
            var renderedString = new StringBuilder();
            var elemStack = new Stack<ElementType>();
            var textStartIndex = 0;
            foreach (var element in elements)
            {
                var textEndIndex = element.StartIndex - 1;
                var length = Math.Max(0, textEndIndex - textStartIndex + 1);
                renderedString.Append(mdString.Substring(textStartIndex, length));
                textStartIndex = element.EndIndex;

                var tagData = TagInfo.TypeToHtmlTag[element.Type];
                if (elemStack.TryPeek(out var type) && type == element.Type)
                {
                    elemStack.Pop();
                    renderedString.Append(tagData.EndTag);
                }
                else
                {
                    elemStack.Push(element.Type);
                    if (!CheckConditions(elemStack.ToList()))
                    {
                        elemStack.Pop();
                        continue;
                    }

                    renderedString.Append(tagData.StartTag);
                    elemStack.Push(element.Type);
                }
            }

            return renderedString.ToString();
        }

        private bool CheckConditions(IReadOnlyList<ElementType> typeList)
        {
            if (typeList.Count > 1 && typeList[typeList.Count - 1] == ElementType.Em 
                                   && typeList[typeList.Count - 2] == ElementType.Strong)
                return false;

            return true;
        }

        public List<ElementData> FindElements(string mdString)
        {
            var elements = new List<ElementData>();
            var mdStarts = TagInfo.MdToType.Keys;
            var maxBufferLength = mdStarts.Max(e => e.Length);
            var i = 0;
            while (i < mdString.Length)
            {
                var bufferLength = Math.Min(maxBufferLength, mdString.Length - i);
                var buffer = mdString.Substring(i, bufferLength);

                var isMdTagInBuf = new Dictionary<string, bool>();
                foreach (var mdTagStart in mdStarts)
                    isMdTagInBuf[mdTagStart] = buffer.StartsWith(mdTagStart);

                var mdTag = isMdTagInBuf.Where(y => y.Value).Max(y => y.Key);
                if (mdTag == null)
                {
                    i++;
                    continue;
                }

                var (type, end) = TagInfo.MdToType[mdTag];
                elements.Add(new ElementData(type, i, i + end.Length));
                i += end.Length;
            }

            return elements;
        }
    }

    static class TagInfo
    {
        public static readonly Dictionary<string, Tuple<ElementType, string>> MdToType = new Dictionary<string, Tuple<ElementType, string>>
        {
            {"_", Tuple.Create(ElementType.Em, "_")},
            {"__", Tuple.Create(ElementType.Strong, "__")}
        };

        public static readonly Dictionary<ElementType, HtmlTagData> TypeToHtmlTag 
            = new Dictionary<ElementType, HtmlTagData>
            {
                {ElementType.Em, new HtmlTagData("<em>", "</em>")},
                {ElementType.Strong, new HtmlTagData("<strong>", "</strong>")}
            };
    }
    public class ElementData
    {
        public readonly ElementType Type;
        public readonly int StartIndex;
        public readonly int EndIndex;

        public ElementData(ElementType type, int startIndex, int endIndex)
        {
            Type = type;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }

    public enum ElementType
    {
        Em,
        Strong
    }

    class HtmlTagData
    {
        public readonly string StartTag;
        public readonly string EndTag;

        public HtmlTagData(string startTag, string endTag)
        {
            StartTag = startTag;
            EndTag = endTag;
        }
    }
}