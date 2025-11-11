namespace Markdown.Dto
{
    public class Token
    {
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public TokenRole Role { get; private set; }
        public TokenPosition Position { get; private set; }

        public Token(string value, TokenType type, TokenRole role, TokenPosition position)
        {
            Value = value;
            Type = type;
            Role = role;
            Position = position;
        }

        public Token(string value, TokenType type, bool canOpen, bool canClose)
        {
            Value = value;
            Type = type;
            DetermineRole(canOpen, canClose);
            DeterminePosition(canOpen, canClose);
        }
        
        private void DetermineRole(bool canOpen, bool canClose)
        {
            Determine(canOpen, canClose, v => Role = v, 
                TokenRole.Both, TokenRole.Open, TokenRole.Close);
        }

        private void DeterminePosition(bool canOpen, bool canClose)
        {
            Determine(canOpen, canClose, v => Position = v, 
                TokenPosition.Inside, TokenPosition.Begin, TokenPosition.End);
        }
        
        private void Determine<T>(bool canOpen, bool canClose, Action<T> setter, T both, T open, T close)
        {
            if (canOpen == canClose)
            {
                setter(both);
            }
            else if (canOpen)
            {
                setter(open);
            }
            else
            {
                setter(close);
            }
        }
    }
}