using System;
using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class EmphasisHandler : HtmlTagHandler
    {
        public override string Handle(ITokenNode tokenNode)
        {
            if (tokenNode.Type == MdSpecification.Emphasis)
            {
                return tokenNode.PairType == TokenPairType.Open ? "<ul>" : "</ul>";
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