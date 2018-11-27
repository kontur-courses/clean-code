using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Renderers
{
    public class Renderer : IRenderer
    {
        private readonly TagHandler tagHandler;

        public Renderer(TagHandler tagHandler)
        {
            this.tagHandler = tagHandler;
        }

        public string Render(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentException("Given tag can't be null", nameof(tag));
            }

            return string.Join("", GetNextTag(tag));
        }

        private IEnumerable<string> GetNextTag(Tag root)
        {
            yield return tagHandler.Handle(root, isOpeningTag: true);

            foreach (var tag in root.Tags ?? Enumerable.Empty<Tag>())
            {
                foreach (var subtag in GetNextTag(tag))
                {
                    yield return subtag;
                }
            }

            yield return tagHandler.Handle(root, isOpeningTag: false);
        }
    }
}