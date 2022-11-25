using Markdown.Markdown;


namespace Markdown.Tokens
{
    public static class TokenParser
    {
        public static Token[] GetTokens(Token[] ArrayMdTokens, int endIndex)
        {
            
            if (endIndex <= 0)
                throw new ArgumentException("End index must be positive and greater than zero");
            var MdTokens = ArrayMdTokens.ToList();
            if (MdTokens.Select(x => x.Length).Contains(0))
                throw new ArgumentNullException("Token list must not contain zero lenght tokens");
            var tokens = new List<Token>();
            var token = new Token(0, 0);
            if (MdTokens.Count != 0)
            {
                var firstToken = new Token(0, 0).GetTokenBetween(MdTokens[0]);
                if (firstToken.Length > 0)
                    tokens.Add(firstToken);
                token = MdTokens[0];
            }
            for (var i = 0; i < MdTokens.Count; i++)
            {
                if (i > 0)
                    CreateTextToken(MdTokens, i, tokens, token);
                token = MdTokens[i];
                tokens.Add(token);
            }
            tokens.Add(token.GetTokenBetween(new Token(endIndex, 0)));
            return tokens.ToArray();
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

