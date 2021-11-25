using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class ItalicToken : IToken
    {
        public const string MdTag = "_";

        public string Value => "_";
        public TokenType Type => TokenType.Italics;
        public int Position { get; }
        public bool IsOpening { get; set; }
        public bool ShouldShowValue { get; set; }
        public bool ShouldBeIgnored => false;
        public int SkipLength => 0;
        public string OpeningTag => "<em>";
        public string ClosingTag => "</em>";
        public bool ShouldBeClosed => false;

        public ItalicToken(int position, bool isOpening, bool shouldBeSkipped)
        {
            Position = position;
            IsOpening = isOpening;
            ShouldShowValue = shouldBeSkipped;
        }

        public ItalicToken(int position)
        {
            Position = position;
        }

        public void SetIsOpening(TokenBuilder tokenBuilder, string markdown, 
            HashSet<TokenType> tokens)
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

        public void Validate(string markdown, IEnumerable<IToken> tokens)
        {
        }
    }
}
