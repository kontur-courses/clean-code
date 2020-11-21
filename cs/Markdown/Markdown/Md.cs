using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private HashSet<string> tags = new HashSet<string> {"__", "_", "# "};

        private Dictionary<MarkupType, string> markupToHtmlTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "strong"},
            {MarkupType.Italic, "em"},
            {MarkupType.Header, "h1"}
        };

        private Dictionary<MarkupType, string> markupToMdTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "__"},
            {MarkupType.Italic, "_"},
            {MarkupType.Header, "# "}
        };

        private HashSet<string> singleTags = new HashSet<string> {"# "};

        private MarkupProcessor markupProcessor;

        public Md()
        {
            markupProcessor = new MarkupProcessor(markupToHtmlTag, markupToMdTag, singleTags);
        }

        public string MarkdownToHtml(string text)
        {
            var htmlText = new StringBuilder();
            var tokenizer = new Tokenizer(text, markupProcessor);
            var currentMarkup = new Stack<MarkupType>();
            var tokens = tokenizer.GetTokens();
            foreach (var token in tokens)
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
                {
                    var tokenText = text.Substring(token);
                    if (tokenText.EndsWith(Environment.NewLine))
                    {
                        htmlText.Append(tokenText.Substring(0,
                            tokenText.Length - Environment.NewLine.Length));
                        while (currentMarkup.Any())
                        {
                            var lastMarkup = currentMarkup.Peek();
                            if (!markupProcessor.IsSingleTag(lastMarkup))
                                break;
                            htmlText.Append(markupProcessor.GetClosingTag(lastMarkup));
                            currentMarkup.Pop();
                        }

                        htmlText.Append(Environment.NewLine);
                    }
                    else
                    {
                        htmlText.Append(tokenText);
                    }
                }
            }

            while (currentMarkup.Any())
            {
                htmlText.Append(markupProcessor.GetClosingTag(currentMarkup.Pop()));
            }

            return htmlText.ToString();
        }
    }
}