using System;
using Markdown.Interfaces;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public IMarkupConverter MarkupConverter { get; set; }

        public Md(IMarkupConverter markupConverter)
        {
            this.MarkupConverter = markupConverter;
        }

        public string Render(string markDownText)
        {
            if (MarkupConverter == null)
                throw new Exception("MarkupConverter is null");

            var tokenizer = new Tokenizer();

            var buffer = new StringBuilder();

            foreach (var token in markDownText.Split('\n').Select(tokenizer.Parse))
            {
                if (token.Count() > 0)
                {
                    buffer.Append(MarkupConverter.Convert(token));
                    buffer.Append('\n');
                }
            }

            return buffer.ToString();
        }
    }
}