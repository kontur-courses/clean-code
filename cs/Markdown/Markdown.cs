using System;

namespace Markdown
{
    public class Markdown
    {
        private IParser parser;

        public Markdown(IParser parser)
        {
            this.parser = parser;
        }

        public string Render(string markdownText)
        {
            throw new NotImplementedException();
        }
    }
}