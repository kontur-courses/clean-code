using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private HashSet<string> tags = new HashSet<string> {"__", "_", "# "};

        private readonly Dictionary<MarkupType, string> markupToHtmlTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "strong"},
            {MarkupType.Italic, "em"},
            {MarkupType.Header, "h1"}
        };

        private readonly Dictionary<MarkupType, string> markupToMdTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "__"},
            {MarkupType.Italic, "_"},
            {MarkupType.Header, "# "}
        };

        private readonly HashSet<string> singleTags = new HashSet<string> {"# "};

        private readonly MarkupProcessor markupProcessor;

        public Md()
        {
            markupProcessor = new MarkupProcessor(markupToHtmlTag, markupToMdTag, singleTags);
        }

        private StringBuilder CloseSingleTags(Stack<MarkupType> markup)
        {
            var sb = new StringBuilder();
            while (markup.Any())
            {
                var lastMarkup = markup.Peek();
                if (!markupProcessor.IsSingleTag(lastMarkup))
                    break;
                sb.Append(markupProcessor.GetClosingTag(lastMarkup));
                markup.Pop();
            }

            return sb;
        }

        private StringBuilder CloseAllTags(Stack<MarkupType> markup)
        {
            var sb = new StringBuilder();
            while (markup.Any())
            {
                sb.Append(markupProcessor.GetClosingTag(markup.Pop()));
            }

            return sb;
        }

        private StringBuilder AddMarkup(string text, Stack<MarkupType> markup, Token token)
        {
            var sb = new StringBuilder();
            if (markup.Any() &&
                markup.Peek() == markupProcessor.GetMarkupType(text, token))
            {
                sb.Append(markupProcessor.GetClosingTag(text, token));
                markup.Pop();
            }
            else
            {
                sb.Append(markupProcessor.GetOpeningTag(text, token));
                markup.Push(markupProcessor.GetMarkupType(text, token));
            }

            return sb;
        }

        private StringBuilder AddRawText(string text, Stack<MarkupType> markup, Token token)
        {
            var sb = new StringBuilder();
            var tokenText = text.Substring(token);
            if (tokenText.EndsWith(Environment.NewLine))
            {
                sb.Append(tokenText.Substring(0,
                    tokenText.Length - Environment.NewLine.Length));
                sb.Append(CloseSingleTags(markup));
                sb.Append(Environment.NewLine);
            }
            else
                sb.Append(tokenText);

            return sb;
        }

        public string MarkdownToHtml(string text)
        {
            var htmlText = new StringBuilder();
            var tokenizer = new Tokenizer(text, markupProcessor);
            var currentMarkup = new Stack<MarkupType>();
            var tokens = tokenizer.GetTokens();
            foreach (var token in tokens)
            {
                htmlText.Append(token.IsMarkup
                    ? AddMarkup(text, currentMarkup, token)
                    : AddRawText(text, currentMarkup, token));
            }

            htmlText.Append(CloseAllTags(currentMarkup));
            return htmlText.ToString();
        }
    }
}