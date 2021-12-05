using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class LinkNode : INode
    {
        public NodeCondition Condition { get; private set; }

        private List<INode> urlChildren = new();
        private List<INode> headerChildren = new();
        private bool urlWasClosed = false;
        private bool urlWasOpened = false;
        private bool headerWasClosed = false;

        public bool TryOpen(Stack<INode> openedNodes, List<IToken> tokens, ref int parentTokenPosition)
        {
            if (tokens[parentTokenPosition] is OpeningSquareBracketToken)
            {
                parentTokenPosition++;
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
            if (newToken is ClosingSquareBracketToken)
            {
                headerWasClosed = true;
            }

            if (newToken is ClosingRoundBracketToken)
            {
                urlWasClosed = true;
            }

            if (newToken is OpeningRoundBracketToken)
            {
                urlWasOpened = true;
            }

            if (headerWasClosed && urlWasOpened && urlWasClosed)
            {
                Condition = NodeCondition.Closed;
            }
        }

        // var newToken = tokens[anotherTokenPosition];
        // if ((urlWasClosed || urlWasOpened) && !headerWasClosed ||
        //     urlWasClosed && !urlWasOpened)
        //     return true;
        // if (headerWasClosed && newToken is ClosingSquareBracketToken or OpeningSquareBracketToken)
        //     return true;
        // if (urlWasClosed && newToken is ClosingRoundBracketToken)
        //     return true;
        // if (urlWasOpened && newToken is OpeningRoundBracketToken)
        //     return true;
        // if (headerWasClosed && tokens[anotherTokenPosition] is SpaceToken)
        //     return true;
        // return false;

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
    }
}