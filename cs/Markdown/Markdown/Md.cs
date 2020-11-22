﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private HashSet<string> tags = new HashSet<string> {"__", "_", "# "};
        public static char EscapeSymbol { get; } = '\\';

        private readonly Dictionary<MarkupType, string> markupToHtmlTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "strong"},
            {MarkupType.Italic, "em"},
            {MarkupType.Header, "h1"},
        };

        private readonly Dictionary<MarkupType, string> markupToMdTag = new Dictionary<MarkupType, string>
        {
            {MarkupType.Bold, "__"},
            {MarkupType.Italic, "_"},
            {MarkupType.Header, "# "},
        };

        private readonly HashSet<string> singleTags = new HashSet<string> {"# "};

        private readonly MarkupProcessor markupProcessor;
        private Tokenizer tokenizer;

        public Md()
        {
            markupProcessor = new MarkupProcessor(markupToHtmlTag, markupToMdTag, singleTags);
        }

        public string MarkdownToHtml(string text)
        {
            var htmlText = new StringBuilder();
            tokenizer = new Tokenizer(text, markupProcessor);
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

            while (markup.TryPop(out var tag))
            {
                sb.Append(markupProcessor.GetClosingTag(tag));
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

        private string UnescapeToken(string text, Token token)
        {
            if (token.Length == 0)
                return "";
            var sb = new StringBuilder();
            if (text[token.Start] != EscapeSymbol)
                sb.Append(text[token.Start]);
            var i = token.Start + 1;
            while (i < token.End + 1)
            {
                if (text[i - 1] == EscapeSymbol)
                {
                    if (tags.Contains(text[i].ToString()) || text[i] == EscapeSymbol)
                    {
                        if (sb.Length > 0)
                            sb.Length--;
                        sb.Append(text[i]);
                        if (text[i] == EscapeSymbol)
                            i++;
                    }
                    else
                        sb.Append(text[i]);
                }
                else
                    sb.Append(text[i]);

                i++;
            }

            return sb.ToString();
        }

        private StringBuilder AddRawText(string text, Stack<MarkupType> markup, Token token)
        {
            var sb = new StringBuilder();
            var tokenText = UnescapeToken(text, token);
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
    }
}