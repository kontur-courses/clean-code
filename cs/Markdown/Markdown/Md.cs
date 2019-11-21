using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public Dictionary<string, MdElement> elementSigns;
        private Tokenizer tokenizer;

        public Md()
        {
            elementSigns = new Dictionary<string, MdElement>();
            AddStandartElements();
            tokenizer = new Tokenizer(elementSigns);
        }

        private void AddStandartElements()
        {
            AddElement(new MdElement("_", "<em>", true));
            AddElement(new MdElement("*", "<em>", true));
            AddElement(new MdElement("__", "<strong>", true));
            AddElement(new MdElement("**", "<strong>", true));
            AddElement(new MdElement("+", "<ins>", true));
            AddElement(new MdElement("-", "<del>", true));
            AddElement(new MdElement("'", "<code>", true));
            AddElement(new MdElement("^", "<p>", true));
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var tokens = tokenizer.Tokenize(text);
            var renderedText = new StringBuilder();
            var result = tokens
                .Select(token => GetTokenInterpretation(token));
            return string.Join("", result);
        }

        private String GetTokenInterpretation(Token token) => IsTokenMdAndClosed(token) ?
                    GetHtmlTag(token) : token.Value.ToString();

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
