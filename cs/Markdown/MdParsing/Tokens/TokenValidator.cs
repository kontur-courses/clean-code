using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.MdParsing.Tokens
{
    public static class TokenValidator
    {
        public static bool IsTokenTagOpen(TagType tagType, List<Token> tokens, int index)
        {
            switch (tagType)
            {
                case TagType.Italic:
                    return IsBoldOrItalicOpen(tokens, index) ||
                           (!IsPositionEven(tokens, index) && IsAmountInOneWordEven(tokens, index));
                case TagType.Bold:
                    return IsBoldOrItalicOpen(tokens, index);
                case TagType.Header:
                    return IsHeaderOpen(index);
                default:
                    return true;
            }
        }

        private static bool IsPositionEven(List<Token> tokens, int index)
        {
            var currentPosition = index;
            var countBefore = 0;
            while (currentPosition - 1 >= 0 && tokens[currentPosition - 1].TokenType is not TokenType.Space)
            {
                if (tokens[currentPosition].TokenType is TokenType.Tag &&
                    tokens[currentPosition].Content == tokens[index].Content)
                    countBefore++;
                currentPosition--;
            }

            if (tokens[currentPosition].TokenType is TokenType.Tag &&
                tokens[currentPosition].Content == tokens[index].Content)
                countBefore++;

            return countBefore % 2 == 0;
        }

        private static bool IsHeaderOpen(int index) => index == 0;

        public static bool IsValidTagToken(List<Token> tokens, int index)
        {
            return tokens[index].TagType switch
            {
                TagType.Italic => IsValidItalic(tokens, index),
                TagType.Bold => IsValidBold(tokens, index),
                _ => true
            };
        }

        public static bool OrderIsCorrect(Stack<Token> openedTokens, Token token)
        {
            return token.TagType != TagType.Bold || openedTokens.All(x => x.TagType != TagType.Italic);
        }

        private static bool IsValidBold(List<Token> tokens, int index)
        {
            return IsBoldOrItalicOpen(tokens, index) ^ IsBoldOrItalicClose(tokens, index);
        }

        private static bool IsBoldOrItalicClose(List<Token> tokens, int index)
        {
            return index - 1 > 0 && tokens[index - 1].TokenType is not TokenType.Space &&
                   (index + 1 >= tokens.Count || tokens[index + 1].Content is " " or "# ");
        }
        private static bool IsBoldOrItalicOpen(List<Token> tokens, int index)
        {
            return index + 1 < tokens.Count && tokens[index + 1].TokenType is not TokenType.Space &&
                   (index - 1 < 0 || tokens[index - 1].TokenType is TokenType.Space or TokenType.Tag);
        }

        private static bool IsValidItalic(List<Token> tokens, int index)
        {
            return !IsNearNumber(tokens, index) && (IsValidBold(tokens, index) || IsAmountInOneWordEven(tokens, index));
        }

        private static bool IsAmountInOneWordEven(List<Token> tokens, int index)
        {
            var currentPosition = index;
            var neededTag = tokens[index].Content;
            while (currentPosition - 1 >= 0 && tokens[currentPosition - 1].TokenType is not TokenType.Space)
            {
                currentPosition--;
            }

            currentPosition--;
            var amountInWord = 0;
            while (currentPosition + 1 < tokens.Count && tokens[currentPosition + 1].TokenType is not TokenType.Space)
            {
                currentPosition++;
                if (tokens[currentPosition].TokenType is TokenType.Tag &&
                    tokens[currentPosition].Content == neededTag)
                    amountInWord++;
            }

            return amountInWord % 2 == 0;
        }

        private static bool IsNearNumber(List<Token> tokens, int index)
        {
            return index - 1 >= 0 && index + 1 < tokens.Count &&
                   ((tokens[index - 1].TokenType is TokenType.Number &&
                     tokens[index + 1].TokenType is not TokenType.Space) ||
                    (tokens[index + 1].TokenType is TokenType.Number &&
                     tokens[index - 1].TokenType is not TokenType.Space));
        }
    }
}
