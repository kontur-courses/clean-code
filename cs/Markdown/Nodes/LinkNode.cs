using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class LinkNode: INode
    {
        private List<INode> URLChildren = new();
        private List<INode> HeaderChildren = new();
        private bool UrlWasClosed = false;
        private bool URLWasOpened = false;
        private bool HeaderWasClosed = false;
        public bool TryOpen(List<IToken> tokens, ref int parentTokenPosition)
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
            if (!HeaderWasClosed)
            {
                HeaderChildren.Add(child);
            }
            else if (!UrlWasClosed)
            {
                URLChildren.Add(child);
            }
        }

        public void Close()
        {
            this.UrlWasClosed = true;
            this.HeaderWasClosed = true;
        }

        public bool ShouldBeClosedByNewToken(List<IToken> tokens, int anotherTokenPosition)
        {
            if (tokens[anotherTokenPosition] is ClosingSquareBracketToken)
            {
                HeaderWasClosed = true;
            }

            if (tokens[anotherTokenPosition] is ClosingRoundBracketToken)
            {
                UrlWasClosed = true;
            }

            if (tokens[anotherTokenPosition] is OpeningRoundBracketToken)
            {
                URLWasOpened = true;
            }

            return HeaderWasClosed && URLWasOpened && UrlWasClosed;
        }

        public bool CannotBeClosed(List<IToken> tokens, int anotherTokenPosition)
        {
            var newToken = tokens[anotherTokenPosition];
            if ((UrlWasClosed || URLWasOpened) && !HeaderWasClosed ||
                UrlWasClosed && !URLWasOpened)
                return true;
            if (HeaderWasClosed && newToken is ClosingSquareBracketToken or OpeningSquareBracketToken)
                return true;
            if (UrlWasClosed && newToken is ClosingRoundBracketToken)
                return true;
            if (URLWasOpened && newToken is OpeningRoundBracketToken)
                return true;
            if (HeaderWasClosed && tokens[anotherTokenPosition] is SpaceToken)
                return true;
            return false;
        }

        public bool ShouldBeClosedWhenParagraphEnds()
        {
            return false;
        }

        public StringBuilder GetNodeBuilder()
        {

            if (HeaderWasClosed && URLWasOpened && UrlWasClosed)
            {
                return GetClosedNodeBuilder();
            }

            return GetOpenedNodeBuilder();
        }

        private StringBuilder GetClosedNodeBuilder()
        {
            var url = string.Join("", URLChildren.Select(x => x.GetNodeBuilder()));
            var urlWithoutBrackets = url[2..];
            var header = string.Join("", HeaderChildren.Select(x => x.GetNodeBuilder()));
            var href = $"<a href=\"{urlWithoutBrackets}\">{header}</a>";
            return new StringBuilder(href);
        }
        
        private StringBuilder GetOpenedNodeBuilder()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            var url = string.Join("", URLChildren.Select(x => x.GetNodeBuilder()));
            var header = string.Join("", HeaderChildren.Select(x => x.GetNodeBuilder()));
            builder.Append(url);
            builder.Append(header);
            return builder;
        }
    }
}