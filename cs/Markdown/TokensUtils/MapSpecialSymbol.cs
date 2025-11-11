namespace Markdown.TokensUtils
{
    using Markdown.Dto;

    public static class MapSpecialSymbol
    {
        public static Token? Specialize(char c, string line, int index)
        {
            return c switch
            {
                '[' => CreateLinkToken(line, index, true),
                ')' => CreateLinkToken(line, index, false),
                '_' when index + 1 < line.Length && line[index + 1] == '_' =>
                    CreateUnderscoreToken(line, index, true),
                '_' => CreateUnderscoreToken(line, index, false),
                '#' when index + 1 < line.Length && line[index + 1] == ' ' =>
                    CreateHeaderToken(line, index),
                '\\' when index + 1 < line.Length =>
                    new Token(line[index].ToString(), TokenType.Escape, TokenRole.None, TokenPosition.None),
                _ => null
            };
        }

        private static CharContext GetCharContext(string line, int index, int tokenLength)
        {
            var prevChar = index > 0 ? line[index - 1] : (char?)null;
            var nextChar = index + tokenLength < line.Length ? line[index + tokenLength] : (char?)null;
            var canOpen = nextChar != null && !char.IsWhiteSpace(nextChar.Value);
            var canClose = prevChar != null && !char.IsWhiteSpace(prevChar.Value);
            return new CharContext(nextChar, canOpen, canClose);
        }

        private static Token CreateUnderscoreToken(string line, int index, bool isStrong)
        {
            return CreateToken(line, index, isStrong ? "__" : "_",
                isStrong ? TokenType.Strong : TokenType.Italic,
                charContext => (charContext.CanOpen, charContext.CanClose));
        }
        

        private static Token CreateLinkToken(string line, int index, bool isOpen)
        {
            return  CreateToken(line, index, isOpen ? "[" : ")",
                TokenType.Link,
                charContext => (isOpen && charContext.CanOpen, !isOpen && charContext.CanClose));
        }
       

        private static Token CreateHeaderToken(string line, int index)
        {
            return CreateToken(line, index, "#",
                TokenType.Header,
                ctx =>
                {
                    var canOpenClose = ctx.Next != null && char.IsWhiteSpace(ctx.Next.Value);
                    return (canOpenClose, canOpenClose);
                });
        }
        

        private static Token CreateToken(
            string line,
            int index,
            string value,
            TokenType type,
            Func<CharContext, (bool canOpen, bool canClose)> roleSelector)
        {
            var ctx = GetCharContext(line, index, value.Length);
            var (canOpen, canClose) = roleSelector(ctx);
            return new Token(value, type, canOpen, canClose);
        }
    }
}