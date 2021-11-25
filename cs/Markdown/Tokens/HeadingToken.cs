using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokens
{
    public class HeadingToken : IToken
    {
        public const string MdTag = "# ";
        public static readonly string[] NewParagraphSymbols = {"\r", "\n"};

        public string Value => "# ";
        public TokenType Type => TokenType.Heading;
        public int Position { get; private set; }
        public bool IsOpening { get; set; }
        public bool ShouldShowValue { get; set; }
        public bool ShouldBeIgnored { get; private set; }
        public int SkipLength => 0;
        public string OpeningTag => "<h1>";
        public string ClosingTag => "</h1>";

        public HeadingToken(int position, bool isOpening)
        {
            Position = position;
            IsOpening = isOpening;
        }

        public void SetIsOpening(string markdown, HashSet<TokenType> tokens)
        {
            var isNotFirst = Position != 0;
            var isNotLast = Position + Value.Length < markdown.Length;
            if (IsOpening)
            {
                if (!tokens.Contains(Type))
                {
                    var notValid = false;
                    if (isNotFirst)
                        notValid = NewParagraphSymbols.All(c => markdown[Position - 1] != c[0]);
                    ShouldShowValue = notValid;
                    if(!ShouldShowValue)
                        tokens.Add(Type);
                }
                else
                    ShouldShowValue = true;
            }
            else
            {
                if (!tokens.Contains(Type))
                    ShouldBeIgnored = true;
                else
                    Position--;
                tokens.Clear();
            }
        }

        public void Validate(string markdown, IEnumerable<IToken> tokens)
        {
        }
    }
}
