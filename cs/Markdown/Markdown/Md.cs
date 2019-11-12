using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public Dictionary<char, MdElement> elementSigns;
        private Tokenizer tokenizer;

        public Md()
        {
            elementSigns = new Dictionary<char, MdElement>();
            AddElement(new MdElement('_', "<em>", true));
            AddElement(new MdElement('*', "<strong>", true));
            tokenizer = new Tokenizer(elementSigns);
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var tokens = tokenizer.Tokenize(text);
            var renderedText = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.MdElement)
                {
                    if (token.IsClosed)
                    {
                        var MdElement = token.MdType;
                        var tag = token.MdPosition == MdPosition.Opening ?
                            MdElement.HtmlTag : MdElement.HtmlTagClose;
                        renderedText.Append(tag);
                    }
                    else
                        renderedText.Append(token.Value);
                }
                else
                    renderedText.Append(token.Value);
            }
            return renderedText.ToString();
        }

        public void AddElement(MdElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            elementSigns.Add(element.MdTag, element);
        }
    }
}
