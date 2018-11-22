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

        public string Render(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentException("Given tag can't be null", nameof(tag));
            }

            return string.Join("", GetNextHtmlTag(tag));
        }

        private IEnumerable<string> GetNextHtmlTag(Tag root)
        {
            if (root.Type != "root")
            {
                yield return tagHandler.Handle(root);
            }

            if (root.Tags == null)
            {
                yield break;
            }

            foreach (var tag in root.Tags)
            {
                foreach (var subtag in GetNextHtmlTag(tag))
                {
                    yield return subtag;
                }
            }

            if (root.Type != "root")
            {
                yield return tagHandler.Handle(root);
            }
        }
    }
}