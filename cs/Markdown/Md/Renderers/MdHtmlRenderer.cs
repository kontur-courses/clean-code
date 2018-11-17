using System;
using System.Collections;
using System.Text;

namespace Markdown.Md.Renderers
{
    public class MdHtmlRenderer : IRenderer
    {
        private readonly IDictionary htmlRules;

        public MdHtmlRenderer(IDictionary htmlRules)
        {
            this.htmlRules = htmlRules;
        }

        public string Render(MdToken[] tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentException("Given tokens can't be null", nameof(tokens));
            }

            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var type = htmlRules[token.Type];

                result.Append(type);

                if (token.Type == MdType.Text)
                {
                    result.Append(token.Value);
                }
            }

            return result.ToString();
        }
    }
}