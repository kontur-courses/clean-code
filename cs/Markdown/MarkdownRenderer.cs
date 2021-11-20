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
            return RenderMatches(matches);
        }

        private string RenderMatches(IEnumerable<TokenMatch> matches)
        {
            MarkPositions(matches);
            return ConvertText();
        }

        private string ConvertText()
        {
            var builder = new StringBuilder();
            for (var i = 0; i <= text.Length; i++)
            {
                if (TryAppendStartTag(builder, ref i))
                    continue;

                if (TryAppendEndTag(builder, ref i))
                    continue;

                AppendSymbol(builder, ref i);
            }

            return builder.ToString();
        }

        private void AppendSymbol(StringBuilder builder, ref int i)
        {
            if (i >= text.Length)
                return;

            if (escaper.IsEscapeSymbol(i))
            {
                builder.Append(text[i + 1]);
                i++;
            }
            else
            {
                builder.Append(text[i]);
            }
        }

        private bool TryAppendEndTag(StringBuilder builder, ref int i)
        {
            if (!matchEndAtPosition.TryGetValue(i, out var matchAtEnd))
                return false;

            builder.Append(matchAtEnd.Token.TagConverter.HtmlCloseTag);
            i += matchAtEnd.Token.TagConverter.TrimFromEndCount - 1;
            return true;
        }

        private bool TryAppendStartTag(StringBuilder builder, ref int i)
        {
            if (!matchStartAtPosition.TryGetValue(i, out var matchAtStart))
                return false;

            builder.Append(matchAtStart.Token.TagConverter.HtmlOpenTag);
            i += matchAtStart.Token.TagConverter.TrimFromStartCount - 1;
            return true;
        }

        private void MarkPositions(IEnumerable<TokenMatch> matches)
        {
            var matchesList = matches.ToList();
            matchStartAtPosition = matchesList.ToDictionary(match => match.Start, match => match);
            matchEndAtPosition = matchesList.ToDictionary(
                match => match.Start + match.Length - match.Token.Pattern.EndTag.Length,
                match => match);
        }
    }
}