using System.Collections.Generic;
using Markdown.Element;
using Markdown.Token;

namespace Markdown
{
    public class Md
    {
        private readonly List<IElement> elements = new List<IElement>();

        public Md(params IElement[] elements)
        {
            foreach (var element in elements)
            {
                this.elements.Add(element);
            }
        }

        public string Render(string text)
        {
            foreach (var element in elements)
            {
                text = ResultFormatter.Form(TokenParser.Parse(text, element));
            }

            return ResultFormatter.RemoveSlashes(text);
        }
    }
}
