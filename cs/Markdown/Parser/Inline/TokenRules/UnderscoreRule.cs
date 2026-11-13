using Markdown.Parser.Inline.Rules;
using Markdown.Parser.Inline.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.TokenRules
{
    public class UnderscoreRule : ITokenRule
    {
        public bool CanHandle(char c) => c == '_';

        public int Handle(string text, int position, List<Token> tokens, StringBuilder buffer)
        {
            Tokenizer.AddTextToken(tokens, buffer, position);

            var isStrong = position + 1 < text.Length && text[position + 1] == '_';
            PairToken token = isStrong ? new StrongToken(position) : new EmToken(position);

            SetPairFlags(token, text, position, isStrong ? 2 : 1);
            tokens.Add(token);

            return isStrong ? 2 : 1;
        }

        private void SetPairFlags(PairToken token, string text, int startIndex, int length)
        {
            char? prev = startIndex > 0 ? text[startIndex - 1] : null;
            char? next = startIndex + length < text.Length ? text[startIndex + length] : null;

            if (next.HasValue && !char.IsWhiteSpace(next.Value)) token.CanOpen = true;
            if (prev.HasValue && !char.IsWhiteSpace(prev.Value)) token.CanClose = true;

            if (prev.HasValue && next.HasValue && char.IsDigit(prev.Value) && char.IsDigit(next.Value))
            {
                token.CanOpen = false;
                token.CanClose = false;
            }

            token.IsInsideWord = prev.HasValue && next.HasValue &&
                                 char.IsLetter(prev.Value) && char.IsLetter(next.Value);
        }
    }
}
