using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer
    {
        private readonly string text;
        private Dictionary<int, TokenMatch> matchStartAtPosition = new();
        private Dictionary<int, TokenMatch> matchEndAtPosition = new();

        public MarkdownRenderer(string text)
        {
            this.text = text;
        }

        public string Render()
        {
            var matches = new TokenReader(text, MarkdownTokensFactory.GetTokens()).FindAll();
            return RenderMatches(matches);
        }

        internal string RenderMatches(IEnumerable<TokenMatch> matches)
        {
            MarkPositions(matches);
            return ConvertText();
        }

        private string ConvertText()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                if (TryAppendStartTag(builder, ref i))
                    continue;

                if (TryAppendEndTag(builder, ref i))
                    continue;

                builder.Append(text[i]);
            }

            return builder.ToString();
        }

        private bool TryAppendEndTag(StringBuilder builder, ref int i)
        {
            if (!matchEndAtPosition.TryGetValue(i, out var matchAtEnd))
                return false;

            builder.Append(matchAtEnd.Token.TagConverter.CloseTag);
            i += matchAtEnd.Token.Pattern.TagLength - 1;
            return true;
        }

        private bool TryAppendStartTag(StringBuilder builder, ref int i)
        {
            if (!matchStartAtPosition.TryGetValue(i, out var matchAtStart))
                return false;

            builder.Append(matchAtStart.Token.TagConverter.OpenTag);
            i += matchAtStart.Token.Pattern.TagLength - 1;
            return true;
        }

        private void MarkPositions(IEnumerable<TokenMatch> matches)
        {
            var matchesList = matches.ToList();
            matchStartAtPosition = matchesList.ToDictionary(match => match.Start, match => match);
            matchEndAtPosition = matchesList.ToDictionary(
                match => match.Start + match.Length - match.Token.Pattern.TagLength,
                match => match);
        }
    }
}