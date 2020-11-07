using System;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        private readonly Tag headerTag, boldTag, italicTag;

        public Md()
        {
            headerTag = new Tag("h1");
            boldTag = new Tag("strong");
            italicTag = new Tag("em");
        }

        public string Render(string markdown)
        {
            var lines = markdown.Split('\n');
            var renderedLines = lines
                .Select(RenderLine);
            return string.Join('\n', renderedLines);
        }

        private string RenderLine(string line)
        {
            throw new NotImplementedException();
        }
    }
}