using System;
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

        private readonly TokenEscaper escaper;
        private readonly TokenReader reader;

        public MarkdownRenderer()
        {
            escaper = new TokenEscaper(tokensToRender);
            reader = new TokenReader(tokensToRender);
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var matches = reader.FindAll(text);
            var matchesMap = new MatchesMap(matches.ToList());
            return ConvertText(text, matchesMap);
        }

        private string ConvertText(string text, MatchesMap matchesMap)
        {
            var builder = new StringBuilder();
            var context = new Context(text);
            for (var i = 0; i <= text.Length; i++)
            {
                context.Index = i;
                var nextPart = GetNextPart(matchesMap, context);
                builder.Append(nextPart.Text);
                i = nextPart.Index;
            }

            return builder.ToString();
        }

        private Context GetNextPart(MatchesMap matchesMap, Context context)
        {
            if (matchesMap.TryGetTagReplacerAtPosition(context.Index, out var replacer))
                return new Context(replacer.Tag, context.Index + replacer.TrimLength - 1);

            if (context.Index < context.Text.Length)
                return escaper.IsEscapeSymbol(context)
                    ? new Context(context.Skip(1).CurrentSymbol.ToString(), context.Index + 1)
                    : new Context(context.CurrentSymbol.ToString(), context.Index);

            return new Context("", context.Index);
        }
    }
}