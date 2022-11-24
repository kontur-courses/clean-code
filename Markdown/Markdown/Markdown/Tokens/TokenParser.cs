using Markdown.Markdown;


namespace Markdown.Tokens
{
    public static class TokenParser
    {
        public static List<Token> GetTokens(List<Token> tokenList, int endIndex)
        {
            if (endIndex <= 0)
                throw new ArgumentException("End index must be positive and greater than zero");

            if (tokenList.Select(x => x.Length).Contains(0))
                throw new ArgumentNullException("Token list must not contains zero lenght tokens");
            var tokens = new List<Token>();
            var token = new Token(0, 0);
            if (tokenList.Count != 0)
            {
                var firstToken = new Token(0, 0).GetTokenBetween(tokenList[0]);
                if (firstToken.Length > 0)
                    tokens.Add(firstToken);
                token = tokenList[0];
            }
            for (var i = 0; i < tokenList.Count; i++)
            {
                if (i > 0)
                    CreateTextToken(tokenList, i, tokens, token);
                token = tokenList[i];
                tokens.Add(token);
            }
            tokens.Add(token.GetTokenBetween(new Token(endIndex, 0)));
            return tokens;
        }

        private static void CreateTextToken(List<Token> tokenList, int i, List<Token> tokens, Token token)
        {
            var textToken = tokenList[i - 1].GetTokenBetween(tokenList[i]);
            if (textToken.Length > 0)
                tokens.Add(textToken);
            else if (token.Type == tokenList[i].Type && token.End + 1 == tokenList[i].Position && token.Type != TokenType.Field)
            {
                tokens[i - 1].SetToDefault();
                tokenList[i].SetToDefault();
            }
        }
    }
}

