using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private string[] tags = {"__", "_", "# "};

        private Dictionary<MarkupType, string> markupToTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "strong"},
            {MarkupType.Italic, "em"},
            {MarkupType.Default, ""},
            {MarkupType.Header, "h1"}
        };

        private MarkupProcessor markupProcessor;

        public Md(Dictionary<MarkupType, string> markupToTag)
        {
            markupProcessor = new MarkupProcessor(markupToTag);
        }

        public Md()
        {
        }

        //Вынести эти методы из Md
        public string MarkdownToHtml(string text)
        {
            var htmlText = new StringBuilder();
            var tokenizer = new Tokenizer(text, tags);
            var currentMarkup = new Stack<MarkupType>();

            foreach (var token in tokenizer.GetTokens())
            {
                if (token.IsMarkup)
                {
                    if (currentMarkup.Any() &&
                        currentMarkup.Peek() == markupProcessor.GetMarkupType(text, token))
                    {
                        htmlText.Append(markupProcessor.GetClosingTag(text, token));
                        currentMarkup.Pop();
                    }
                    else
                    {
                        htmlText.Append(markupProcessor.GetOpeningTag(text, token));
                        currentMarkup.Push(markupProcessor.GetMarkupType(text, token));
                    }
                }
                else
                    htmlText.Append(text.Substring(token));
            }

            return htmlText.ToString();
        }
    }
}