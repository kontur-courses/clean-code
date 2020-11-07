using System;
using System.Linq;

namespace Markdown
{
    public class Md
    {
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