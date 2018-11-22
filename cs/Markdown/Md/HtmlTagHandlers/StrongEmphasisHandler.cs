using System;
using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class StrongEmphasisHandler : HtmlTagHandler
    {
        public override string Handle(ITokenNode tokenNode)
        {
            if (tokenNode.Type == MdSpecification.StrongEmphasis)
            {
                return tokenNode.PairType == TokenPairType.Open ? "<strong>" : "</strong>";
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