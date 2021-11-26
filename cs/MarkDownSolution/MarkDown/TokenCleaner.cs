namespace MarkDown
{
    public static class TokenCleaner
    {
        public static void CleanToken(Token token, int textLength)
        {
            IfOpenedOrIncorrectAddNestedToFather(token, textLength);
        }
        
        private static void IfOpenedOrIncorrectAddNestedToFather(Token token, int textLength)
        {
            if (token.IsFullLiner)
            {
                token.SetLength(textLength);
            }
            var nested = token.GetNestedTokens();
            for (int i = 0; i < nested.Count; i++)
            {
                IfOpenedOrIncorrectAddNestedToFather(nested[i], textLength);
            }
            if (!token.Closed || token is BoldToken && token.FatherToken is ItalicToken)
            {
                foreach (var n in nested)
                {
                    token.FatherToken.AddNestedToken(n);
                }
                token.FatherToken.RemoveNestedToken(token);
            }
        }
    }
}
