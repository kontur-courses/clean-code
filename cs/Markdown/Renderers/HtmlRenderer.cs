using System;
using System.Collections.Generic;

namespace Markdown.Renderers
{
    public class HtmlRenderer : IRenderer
    {
        private readonly HtmlTagHandler tagHandler;

        public HtmlRenderer(HtmlTagHandler tagHandler)
        {
            this.tagHandler = tagHandler;
        }

        public string Render(ITokenNode tokenNode)
        {
            if (tokenNode == null)
            {
                throw new ArgumentException("Given tokenNode can't be null", nameof(tokenNode));
            }

            return string.Join("", GetNextHtmlTag(tokenNode.Children));
        }

        public IEnumerable<string> GetNextHtmlTag(ICollection<ITokenNode> children)
        {
            foreach (var child in children)
            {
                yield return tagHandler.Handle(child);

                foreach (var subChild in GetNextHtmlTag(child.Children))
                    yield return subChild;
            }
        }
    }
}