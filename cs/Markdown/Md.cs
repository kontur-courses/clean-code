using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Nodes;

namespace Markdown
{
    public class Md
    {
        private readonly MarkingToNodeParser markingToNodeParser = new MarkingToNodeParser();

        private readonly char[] markingSymbols = new[] {'_', '#'};

        public string Render(string rawString)
        {
            if (rawString == null) 
                throw new ArgumentNullException();
            var paragraphs = GetParagraphs(rawString);
            var builder = new StringBuilder();
            for (var paragraphId = 0; paragraphId < paragraphs.Length; paragraphId++)
            {
                builder.Append(RenderParagraph(paragraphs[paragraphId]));
                if (paragraphId != paragraphs.Length - 1)
                    builder.Append("\n\n");
            }

            return builder.ToString();
        }

        private string RenderParagraph(string paragraph)
        {
            var stringNodeBuilder = new StringBuilder();
            var markingBuilder = new StringBuilder();
            var nodes = new Stack<JoiningNode>();
            nodes.Push(new JoiningNode());
            
            var symbolId = 0;
            while (symbolId < paragraph.Length)
            {
                if (SymbolIsMarking(paragraph[symbolId]))
                {
                    AppendNextSymbol(markingBuilder, paragraph, ref symbolId);
                }
                else
                {
                    AttachNodes(nodes, paragraph, symbolId - 1, markingBuilder, stringNodeBuilder);
                    AppendNextSymbol(stringNodeBuilder, paragraph, ref symbolId);
                }
            }
            AttachNodes(nodes, paragraph, paragraph.Length - 1, markingBuilder, stringNodeBuilder);
            var topNode = GetSummaryTopNode(nodes);
            topNode.AddNode(new StringNode(stringNodeBuilder.ToString()));
            return topNode.GetNodeBuilder().ToString();
        }

        private JoiningNode GetSummaryTopNode(Stack<JoiningNode> nodes)
        {
            var topNode = nodes.Pop();
            while (nodes.Count != 0)
            {
                var node = nodes.Pop();
                node.AddNode(topNode);
                topNode = node;
            }

            return topNode;
        }

        private void AttachNodes(Stack<JoiningNode> nodes,
            string paragraph,
            int lastMarkedSymbolId,
            StringBuilder markingBuilder,
            StringBuilder stringNodeBuilder)
        {
            var marking = BuildMarking(markingBuilder, paragraph, lastMarkedSymbolId);

            CloseAllPossibleTags(nodes, marking, stringNodeBuilder, out marking);
            PushAllPossibleOpeningTags(nodes, marking, stringNodeBuilder, out marking);
            if (!string.IsNullOrEmpty(marking.StringMarking))
                nodes.Peek().AddNode(new StringNode(marking.StringMarking));
        }

        private void CloseAllPossibleTags(Stack<JoiningNode> stack,
            Marking marking,
            StringBuilder stringNodeBuilder,
            out Marking trimmedMarking)
        {
            while (stack.TryPeek(out var node) &&
                   node is TaggedNode tagged &&
                   tagged.ShouldBeClosed(marking))
            {
                node.AddNode(new StringNode(stringNodeBuilder.ToString()));
                tagged.Close(marking, out marking);
                stringNodeBuilder.Clear();
                stack.Pop();
                stack.Peek().AddNode(node);
            }

            trimmedMarking = marking;
        }

        private void AppendNextSymbol(StringBuilder builder, string paragraph, ref int position)
        {
            if (SymbolIsEscaped(paragraph, position))
            {
                builder.Append(paragraph[position + 1]);
                position += 2;
            }
            else
            {
                builder.Append(paragraph[position]);
                position++;
            }
        }
        
        private void PushAllPossibleOpeningTags(Stack<JoiningNode> stack,
            Marking marking,
            StringBuilder stringNodeBuilder,
            out Marking trimmedMarking)
        {
            trimmedMarking = marking;
            while (markingToNodeParser.TryGetOpenedNode(marking, out var node, out marking))
            {
                var lastNode = stack.Peek();
                lastNode.AddNode(new StringNode(stringNodeBuilder.ToString()));
                stringNodeBuilder.Clear();
                stack.Push(node);
                trimmedMarking = marking;
            }
        }

        private Marking BuildMarking(StringBuilder markingBuilder, string line, int lastMarkedSymbol)
        {
            var stringValue = markingBuilder.ToString();
            char? symbolBefore =
                line.InBorders(lastMarkedSymbol - markingBuilder.Length - 1)
                    ? line[lastMarkedSymbol - markingBuilder.Length - 1]
                    : null;
            char? symbolAfter =
                line.InBorders(lastMarkedSymbol + 1)
                    ? line[lastMarkedSymbol + 1]
                    : null;
            markingBuilder.Clear();
            return new Marking(symbolBefore, stringValue, symbolAfter);
        }

        private bool TryReadNextSymbol(string paragraph, ref int position, out char? result)
        {
            result = null;
            if (!paragraph.InBorders(position)) 
                return false;
            if (paragraph[position] == '\\' && paragraph.InBorders(position + 1))
            {
                result = paragraph[position + 1];
                position += 2;
            }
            else
            {
                position++;
                result = paragraph[position];
            }

            return true;
        }

        private bool SymbolIsMarking(char symbol)
        {
            return markingSymbols.Contains(symbol);
        }

        private bool SymbolIsEscaped(string line, int pos)
        {
            return line[pos] == '\\' &&
                   line.InBorders(pos + 1) &&
                   line[pos + 1] == '_';
        }
        
        private string[] GetParagraphs(string str)
        {
            return str.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        }
    }
}