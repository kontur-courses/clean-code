using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    public class ParserNode
    {
        public TagType TypeTag { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public int StartInnerPartPosition { get; }
        public int EndInnerPartPosition { get; }
        public IDictionary<string, string> Attributes => attributes;
        public IEnumerable<ParserNode> Childs => childs;

        private List<ParserNode> childs;
        private readonly Dictionary<string, string> attributes;

        public ParserNode(TagType tagType, int startPosition, int endPosition, IParserRule rule)
        {
            childs = new List<ParserNode>();
            TypeTag = tagType;
            StartPosition = startPosition;
            EndPosition = endPosition;
            StartInnerPartPosition = startPosition + rule.OpenTag.Length;
            EndInnerPartPosition = endPosition - rule.CloseTag.Length;
            attributes = new Dictionary<string, string>();
        }

        public ParserNode(TagType tagType, int startPosition, int endPosition, int startInnerPosition, int endInnerPosition)
        {
            childs = new List<ParserNode>();
            TypeTag = tagType;
            StartPosition = startPosition;
            EndPosition = endPosition;
            StartInnerPartPosition = startInnerPosition;
            EndInnerPartPosition = endInnerPosition;
            attributes = new Dictionary<string, string>();
        }

        public DocumentNode GetDocumentNode(string source, int nestingLevel=0, HashSet<int> escapedPositions=null)
        {
            var result = new DocumentNode(TypeTag, source, StartInnerPartPosition, EndInnerPartPosition, nestingLevel, escapedPositions);
            var stack = new Stack<DocumentNode>();
            var tempStack = new Stack<ParserNode>();
            stack.Push(result);
            tempStack.Push(this);
            while (stack.Count > 0)
            {
                var parserNode = tempStack.Pop();
                var documentNode = stack.Pop();
                foreach (var (key, value) in parserNode.Attributes)
                {
                    documentNode.AddAttribute(key, value);
                }

                foreach (var child in parserNode.Childs)
                {
                    var childDocumentNode = new DocumentNode(child.TypeTag, source, child.StartInnerPartPosition,
                        child.EndInnerPartPosition, documentNode.NestingLevel + 1);
                    stack.Push(childDocumentNode);
                    tempStack.Push(child);

                    documentNode.AddElement(childDocumentNode);
                }
            }

            return result;
        }

        public void AddChild(ParserNode child)
        {
            childs.Add(child);
        }
    }
}