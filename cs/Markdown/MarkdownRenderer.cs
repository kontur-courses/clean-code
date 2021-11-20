using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer
    {
        private readonly List<IToken> tokensToRender = MarkdownTokensFactory.GetTokens().ToList();

        private readonly string text;
        private readonly TokenEscaper escaper;
        private readonly StringBuilder builder = new();
        private int position;
        private Dictionary<int, TokenMatch> matchStartAtPosition = new();
        private Dictionary<int, TokenMatch> matchEndAtPosition = new();

        public MarkdownRenderer(string text)
        {
            this.text = text;
            escaper = new TokenEscaper(text, tokensToRender);
        }

        public string Render()
        {
            var matches = new TokenReader(text, tokensToRender).FindAll();
            MarkPositions(matches);
            return ConvertText();
        }

        private void MarkPositions(IEnumerable<TokenMatch> matches)
        {
            var matchesList = matches.ToList();
            matchStartAtPosition = matchesList.ToDictionary(match => match.Start, match => match);
            matchEndAtPosition = matchesList.ToDictionary(
                match => match.Start + match.Length - match.Token.Pattern.EndTag.Length,
                match => match);
        }

        private string ConvertText()
        {
            for (; position <= text.Length; position++)
            {
                if (TryAppendStartTag())
                    continue;

                if (TryAppendEndTag())
                    continue;

                AppendSymbol();
            }

            return builder.ToString();
        }

        private void AppendSymbol()
        {
            if (position >= text.Length)
                return;

            if (escaper.IsEscapeSymbol(position))
            {
                builder.Append(text[position + 1]);
                position++;
            }
            else
            {
                builder.Append(text[position]);
            }
        }

        private bool TryAppendEndTag()
        {
            if (!matchEndAtPosition.TryGetValue(position, out var matchAtEnd))
                return false;

            builder.Append(matchAtEnd.Token.TagConverter.HtmlCloseTag);
            position += matchAtEnd.Token.TagConverter.TrimFromEndCount - 1;
            return true;
        }

        private bool TryAppendStartTag()
        {
            if (!matchStartAtPosition.TryGetValue(position, out var matchAtStart))
                return false;

            builder.Append(matchAtStart.Token.TagConverter.HtmlOpenTag);
            position += matchAtStart.Token.TagConverter.TrimFromStartCount - 1;
            return true;
        }
    }
}