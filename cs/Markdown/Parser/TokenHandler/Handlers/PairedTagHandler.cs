using Markdown.Token;

namespace Markdown.Parser.TokenHandler.Handlers
{
    public class PairedTagHandler : BaseTokenHandler
    {
        public PairedTagHandler(Delimiter delimiter) : base(delimiter)
        {
        }

        public override bool TryHandle(ParsingContext context, out Token.Token token, out int skip)
        {
            token = null;
            skip = 0;

            var text = context.Text;
            var position = context.Position;

            if (!IsOpeningDelimiter(text, position))
                return false;

            if (HasExcessiveDelimiters(text, position))
                return false;

            if (IsClosingDelimiter(context, text, position, out token, out skip))
                return true;

            return IsOpeningPossible(context, text, position, out token, out skip);
        }

        private bool IsOpeningDelimiter(string text, int position)
        {
            return IsMatch(text, position, Delimiter.Opening);
        }

        private bool HasExcessiveDelimiters(string text, int position)
        {
            var delimiterCount = 0;
            while (position + delimiterCount < text.Length &&
                   IsMatch(text, position + delimiterCount, Delimiter.Opening))
            {
                delimiterCount++;
            }

            return delimiterCount > Delimiter.Opening.Length;
        }

        private bool IsClosingDelimiter(ParsingContext context, string text, int position, out Token.Token token, out int skip)
        {
            token = null;
            skip = 0;

            var isClosingPossible = context.OpenTags.Count > 0 && context.OpenTags.Peek() == Delimiter.Type;
            if (!isClosingPossible)
            {
                return false;
            }

            if (HasLetterOrDigitAfterDelimiter(text, position))
                return false;

            token = CreateClosingToken(position);
            context.OpenTags.Pop();
            skip = Delimiter.Opening.Length;
            return true;
        }

        private bool IsOpeningPossible(ParsingContext context, string text, int position, out Token.Token token, out int skip)
        {
            token = null;
            skip = 0;

            if (!DelimiterOpeningValid(context, text, position))
                return false;

            var closingPos = FindClosingDelimiter(text, position + Delimiter.Opening.Length);
            if (closingPos == -1 || HasWhitespaceBeforeClosing(text, closingPos))
                return false;
            
            var innerText = text.Substring(position + Delimiter.Opening.Length, closingPos - (position + Delimiter.Opening.Length));
            if (innerText.Contains("\n") || innerText.Contains("\r"))
                return false;

            token = CreateOpeningToken(position);
            context.OpenTags.Push(Delimiter.Type);
            skip = Delimiter.Opening.Length;
            return true;
        }

        private bool DelimiterOpeningValid(ParsingContext context, string text, int position)
        {
            if (Delimiter.Opening.Length != 1)
            {
                return true;
            }

            return !HasLetterBefore(text, position) && !HasWhitespaceAfter(text, position);
        }

        private bool HasLetterBefore(string text, int position)
        {
            return position > 0 && char.IsLetterOrDigit(text[position - 1]);
        }

        private bool HasWhitespaceAfter(string text, int position)
        {
            return position + Delimiter.Opening.Length >= text.Length ||
                   char.IsWhiteSpace(text[position + Delimiter.Opening.Length]);
        }

        private bool HasLetterOrDigitAfterDelimiter(string text, int position)
        {
            return position + Delimiter.Opening.Length < text.Length &&
                   char.IsLetterOrDigit(text[position + Delimiter.Opening.Length]);
        }

        private bool HasWhitespaceBeforeClosing(string text, int closingPos)
        {
            return closingPos > 0 && char.IsWhiteSpace(text[closingPos - 1]);
        }

        private Token.Token CreateClosingToken(int position)
        {
            return new Token.Token(Delimiter.Closing, Delimiter.Type, TagState.Close, position);
        }

        private Token.Token CreateOpeningToken(int position)
        {
            return new Token.Token(Delimiter.Opening, Delimiter.Type, TagState.Open, position);
        }

        private int FindClosingDelimiter(string text, int startPos)
        {
            for (var i = startPos; i <= text.Length - Delimiter.Closing.Length; i++)
            {
                if (IsMatch(text, i, Delimiter.Closing))
                    return i;
            }
            return -1;
        }
    }
}