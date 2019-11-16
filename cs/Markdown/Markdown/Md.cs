using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var result = tokens
                .Select(token => IsTokenMdAndClosed(token) ?
                    GetHtmlTag(token) : token.Value.ToString());
            return string.Join("", result);
        }

        private bool IsTokenMdAndClosed(Token token) =>
            token.Type == TokenType.MdElement && token.IsClosed;

        private string GetHtmlTag(Token token) => token.MdPosition == MdPosition.Opening ?
                        token.MdType.HtmlTag : token.MdType.HtmlTagClose;

        public void AddElement(MdElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            elementSigns.Add(element.MdTag, element);
        }
    }
}
