using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class LinkNode : INode
    {
        public NodeCondition Condition { get; private set; }

        private readonly List<INode> urlChildren = new();
        private readonly List<INode> headerChildren = new();
        private bool urlWasClosed;
        private bool urlWasOpened;
        private bool headerWasClosed;

        public bool TryOpen(Stack<INode> parentNodes, CollectionIterator<IToken> tokensIterator)
        {
            if (tokensIterator.GetCurrent() is OpeningSquareBracketToken)
            {
                tokensIterator.Move(1);
                return true;
            }

            return false;
        }

        public void AddChild(INode child)
        {
            if (!headerWasClosed)
            {
                headerChildren.Add(child);
            }
            else if (!urlWasClosed)
            {
                urlChildren.Add(child);
            }
        }

        public void UpdateCondition(IToken newToken)
        {
            if (IsImpossibleToClose(newToken))
            {
                Condition = NodeCondition.ImpossibleToClose;
            }
            else
            {
                if (newToken is ClosingSquareBracketToken)
                {
                    headerWasClosed = true;
                }
                else if (newToken is ClosingRoundBracketToken)
                {
                    urlWasClosed = true;
                }
                else if (newToken is OpeningRoundBracketToken)
                {
                    urlWasOpened = true;
                }

                if (headerWasClosed && urlWasOpened && urlWasClosed)
                {
                    Condition = NodeCondition.Closed;
                }
            }
        }

        public StringBuilder GetNodeBuilder()
        {

            if (headerWasClosed && urlWasOpened && urlWasClosed)
            {
                return GetClosedNodeBuilder();
            }

            return GetOpenedNodeBuilder();
        }

        private StringBuilder GetClosedNodeBuilder()
        {
            var url = string.Join("", urlChildren.Select(x => x.GetNodeBuilder()));
            var urlWithoutBrackets = url[2..];
            var header = string.Join("", headerChildren.Select(x => x.GetNodeBuilder()));
            var href = $"<a href=\"{urlWithoutBrackets}\">{header}</a>";
            return new StringBuilder(href);
        }

        private StringBuilder GetOpenedNodeBuilder()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            var url = string.Join("", urlChildren.Select(x => x.GetNodeBuilder()));
            var header = string.Join("", headerChildren.Select(x => x.GetNodeBuilder()));
            builder.Append(url);
            builder.Append(header);
            return builder;
        }

        private bool IsImpossibleToClose(IToken newToken)
        {
            if (newToken is ParagraphEndToken)
                return true;
            if (!headerWasClosed && (urlWasOpened || urlWasClosed))
                return true;
            if (headerWasClosed && newToken is ClosingSquareBracketToken or OpeningSquareBracketToken)
                return true;
            if (urlWasOpened && newToken is OpeningRoundBracketToken)
                return true;
            if (urlWasClosed && newToken is ClosingRoundBracketToken)
                return true;
            if (headerWasClosed && newToken is SpaceToken)
                return true;
            return false;
        }
    }
}