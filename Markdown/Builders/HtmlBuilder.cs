using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Markdown.IntermediateState;

namespace Markdown.Builders
{
    class HtmlBuilder : ILanguageBuilder
    {
        private Dictionary<TagType, BuilderRule> tagRules;
        private IEnumerable<int> escapedPositions;

        public HtmlBuilder()
        {
            tagRules = new Dictionary<TagType, BuilderRule>
            {
                [TagType.All] = new BuilderRule(TagType.All, "<div>", "</div>"),
                [TagType.H1] = new BuilderRule(TagType.H1, "<h1>", "</h1>"),
                [TagType.H2] = new BuilderRule(TagType.H2, "<h2>", "</h2>"),
                [TagType.H3] = new BuilderRule(TagType.H3, "<h3>", "</h3>"),
                [TagType.H4] = new BuilderRule(TagType.H4, "<h4>", "</h4>"),
                [TagType.H5] = new BuilderRule(TagType.H5, "<h5>", "</h5>"),
                [TagType.H6] = new BuilderRule(TagType.H6, "<h6>", "</h6>"),
                [TagType.Italic] = new BuilderRule(TagType.Italic, "<em>", "</em>"),
                [TagType.Bold] = new BuilderRule(TagType.Bold, "<strong>", "</strong>"),
                [TagType.NoneTag] = new BuilderRule(TagType.NoneTag, "", ""),
                [TagType.Paragraph] = new BuilderRule(TagType.Paragraph, "<p>", "</p>"),
                [TagType.Raw] = new BuilderRule(TagType.Raw, "<pre>", "</pre>")
            };


        }

        public string BuildDocument(DocumentNode parsedDocument, Func<DocumentNode, string> unknownTagAction)
        {
            if (parsedDocument?.SourceDocument is null || parsedDocument.SourceDocument == "")
            {
                return "";
            }

            escapedPositions = parsedDocument.EscapedPositions;

            Stack<DocumentNode> operationsStack = new Stack<DocumentNode>();
            Stack<string> openTags = new Stack<string>();
            Stack<string> resultStack = new Stack<string>();
            operationsStack.Push(parsedDocument);
            while (operationsStack.Count > 0)
            {
                var current = operationsStack.Pop();
                if (current.ContainsInnerElements())
                {
                    resultStack.Push(tagRules[current.TypeTag].CloseTag);
                    openTags.Push(GetOpenTagWithAttributes(current));
                    foreach (var innerElement in current.InnerElements)
                    {
                        operationsStack.Push(innerElement);
                    }
                    continue;
                }
                resultStack.Push(tagRules[current.TypeTag].CloseTag);
                var length = current.EndInnerPartInSource - current.BeginInnerPartInSource;
                resultStack.Push(GetSubstring(parsedDocument.SourceDocument, current.BeginInnerPartInSource, current.EndInnerPartInSource));
                resultStack.Push(GetOpenTagWithAttributes(current));
                var nestingLevel = current.NestingLevel;
                while (operationsStack.Count > 0 && nestingLevel-- > operationsStack.Peek().NestingLevel && openTags.Count > 0)
                    resultStack.Push(openTags.Pop());
            }
            while (openTags.Count > 0)
                resultStack.Push(openTags.Pop());

            var result = new StringBuilder();
            foreach (var element in resultStack)
            {
                result.Append(element);
            }

            return result.ToString();
        }

        private string GetOpenTagWithAttributes(DocumentNode node)
        {
            var builder = new StringBuilder();
            if (node.TypeTag == TagType.NoneTag)
                return "";

            builder.Append(tagRules[node.TypeTag].OpenTag.Split('>')[0]);
            foreach (var (key, value) in node.Attributes)
            {
                builder.Append(" ").Append(key).Append("=\"").Append(value).Append("\"");
            }

            return builder.Append(">").ToString();
        }

        private string GetSubstring(string source, int startPosition, int endPosition)
        {
            var result = new StringBuilder();
            var currentPosition = startPosition;
            foreach (var position in escapedPositions.Where(e => e >= startPosition && e < endPosition))
            {
                result.Append(source.Substring(currentPosition, position - 1 - currentPosition));
                currentPosition = position;
            }

            return result.Append(source.Substring(currentPosition, endPosition - currentPosition)).ToString();
        }
    }
}
