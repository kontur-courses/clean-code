using System;
using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class TextHandler : HtmlTagHandler
    {
        public override string Handle(ITokenNode tokenNode)
        {
            if (tokenNode.PairType == TokenPairType.NotPair)
            {
                return tokenNode.Value;
            }

            if (Successor == null)
            {
                throw new InvalidOperationException(
                    "Can't transfer control to the next chain element because it was null");
            }

            return Successor.Handle(tokenNode);
        }
    }
}