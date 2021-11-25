using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class BoldToken : IToken
    {
        public const string MdTag = "__";

        public string Value => "__";
        public TokenType Type => TokenType.Bold;
        public int Position { get; }
        public bool IsOpening { get; set; }
        public bool ShouldShowValue { get; set; }
        public string OpeningTag => "<strong>";
        public string ClosingTag => "</strong>";
        public bool ShouldBeIgnored => false;
        public int SkipLength => 0;

        public BoldToken(int position)
        {
            Position = position;
        }

        public void Validate(string markdown, IEnumerable<IToken> tokens)
        {

        }

        public void SetIsOpening(string markdown, HashSet<TokenType> tokens)
        {
            var isNotFirst = Position != 0;
            var isNotLast = Position + Value.Length < markdown.Length;

            if (!tokens.Contains(Type))
            {
                if (isNotLast)
                {
                    IsOpening = markdown[Position + Value.Length] != ' ';
                    if (IsOpening)
                        tokens.Add(Type);
                }
                else
                    IsOpening = false;  
            }
            else
            {
                if (isNotFirst)
                {
                    IsOpening = markdown[Position - 1] != ' ' ^ true;
                    if (!IsOpening)
                        tokens.Remove(Type);
                }
                else
                    IsOpening = false;
            }
        }
    }
}
