using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Readers;
using Markdown.Tags;

namespace Markdown
{
    class Md
    {
        private IEnumerable<IReader> readers;

        public Md(IEnumerable<IReader> readers)
        {
            this.readers = readers;
        }

        public string Render(string markdown)
        {
            var tokens = new List<IToken>();
            for (int i = 0; i < markdown.Length; i++)
            {
                var token = readers.Select(reader => reader.ReadToken(markdown, i)).FirstOrDefault(element => element != null);
                if (token == null) continue;
                i = token.Position;
                tokens.Add(token);
            }

            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                result.Append(token.ToHtml());
            }

            return result.ToString();
        }
    }
}
