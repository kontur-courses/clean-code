using Markdown.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Styles
{
    internal abstract class Style
    {
        public readonly Token[] BeginingTokens;
        public readonly Token[] EndingTokens;
        protected StyleBeginToken styleBeginToken;
        protected StyleEndToken styleEndToken;

        public Style(Token[] beginingTokens, Token[] endingTokens, StyleBeginToken styleBeginToken, StyleEndToken styleEndToken)
        {
            BeginingTokens = beginingTokens;
            EndingTokens = endingTokens;
            this.styleBeginToken = styleBeginToken;
            this.styleEndToken = styleEndToken;
        }

        protected bool IsTokensMatch(Token[] tokens1, Token[] tokens2)
        {
            for (int i = 0; i < tokens1.Count(); i++)
            {
                if (tokens1[i].GetType() != tokens2[i].GetType())
                    return false;
            }
            return true;
        }

        private bool ContainsDigits(Word word) => word.ToText().ToArray().Any(c => char.IsDigit(c));

        protected bool FindDigitsForward(int index, List<Token> tokens)
        {
            index++;
            while (index < tokens.Count)
            {
                if (tokens[index] is Space) return false;
                if (tokens[index] is Word word && ContainsDigits(word)) return true;
                index++;
            }
            return false;
        }

        protected bool FindDigitsBackward(int index, List<Token> tokens)
        {
            index--;
            while (index > 0)
            {
                if (tokens[index] is Space) return false;
                if (tokens[index] is Word word && ContainsDigits(word)) return true;
                index--;
            }
            return false;
        }

        protected bool IsBeginingTokens(int index, List<Token> tokens) =>
            index + BeginingTokens.Length + 1 <= tokens.Count &&
            !(tokens[index + BeginingTokens.Length] is Space) &&
            IsTokensMatch(BeginingTokens, tokens.GetRange(index, BeginingTokens.Length).ToArray()) &&
            !FindDigitsBackward(index, tokens) && !FindDigitsForward(index, tokens);

        protected bool IsEndingTokens(int index, List<Token> tokens) =>
            !(tokens[index - 1] is Space) &&
            index + EndingTokens.Length <= tokens.Count &&
            IsTokensMatch(EndingTokens, tokens.GetRange(index, EndingTokens.Length).ToArray()) &&
            !FindDigitsBackward(index, tokens) && !FindDigitsForward(index, tokens);

        protected bool FindEndingTokensIndex(List<Token> tokens, int startIndex, out int endIndex)
        {
            endIndex = default;
            int i = startIndex + BeginingTokens.Length + 1;
            while (i < tokens.Count - EndingTokens.Length + 1)
            {
                if (tokens[i] is StyleEndToken)
                    return false;

                if (IsEndingTokens(i, tokens))
                {
                    endIndex = i;
                    return true;
                }

                i++;
            }
            return false;
        }

        public bool Apply(List<Token> tokens, int beginIndex)
        {
            if (IsBeginingTokens(beginIndex, tokens) && FindEndingTokensIndex(tokens, beginIndex, out int endIndex))
            {
                tokens.RemoveRange(endIndex, EndingTokens.Length);
                tokens.Insert(endIndex, styleEndToken);
                tokens.RemoveRange(beginIndex, BeginingTokens.Length);
                tokens.Insert(beginIndex, styleBeginToken);
                return true;
            }
            return false;
        }
    }
}
