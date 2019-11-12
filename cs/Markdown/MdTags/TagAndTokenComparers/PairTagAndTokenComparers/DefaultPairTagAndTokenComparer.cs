using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers
{
    class DefaultPairTagAndTokenComparer
    {
        private readonly Tag open;
        private readonly Tag close;

        private readonly char escapeSymbol;
        private readonly char ignoreSymbol;

        public DefaultPairTagAndTokenComparer(Tag open, Tag close)
        {
            if (open == null || close == null)
                throw new ArgumentNullException();

            this.open = open;
            this.close = close;

            escapeSymbol = MdSpecialCharacters.Escape;
            ignoreSymbol = ' ';
        }

        public bool IsTokenOpenTag(Token token)
        {
            if (token.Str.Substring(token.StartIndex, token.Count) != open.Value)
                return false;
            if (IgnoreTag(token))
                return false;
            if (token.StartIndex + token.Count >= token.Str.Length)
                return true;
            return token.Str[token.StartIndex + token.Count] != ignoreSymbol;
        }

        public bool IsTokenCloseTag(Token token)
        {
            if (token.Str.Substring(token.StartIndex, token.Count) != close.Value)
                return false;
            if (IgnoreTag(token))
                return false;
            if (token.StartIndex - 1 < 0)
                return true;
            return token.Str[token.StartIndex - 1] != ignoreSymbol;
        }

        private bool IgnoreTag(Token token)
        {
            return ContainsEscapeSymbol(token) || IsTagInsideDigits(token);
        }

        private bool ContainsEscapeSymbol(Token token)
        {
            var right = token.StartIndex + token.Count;
            if (right >= token.Str.Length)
                return false;
            return token.Str[right] == escapeSymbol &&
                !MdSpecialCharacters.IsCharacterEscapedByEscapeCharacter(right, token.Str);
        }

        private bool IsTagInsideDigits(Token token)
        {
            var leftIndex = token.StartIndex - 1;
            var rightIndex = token.StartIndex + token.Count;
            if (leftIndex < 0 || rightIndex >= token.Str.Length)
                return false;
            return char.IsDigit(token.Str[leftIndex]) && char.IsDigit(token.Str[rightIndex]);
        }
    }
}