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
        public int Length => IsOpening ? Value.Length : 1;
        public int SkipLength => 0;
        public string OpeningTag => "<h1>";
        public string ClosingTag => "</h1>";
        public bool ShouldBeClosed { get; set; }

        public HeadingToken(int position, bool isOpening)
        {
            Position = position;
            IsOpening = isOpening;
        }

        public void SetIsOpening(TokenBuilder tokenBuilder, string markdown, 
            HashSet<TokenType> tokens)
        {
            var isNotFirst = Position != 0;
            var isNotLast = Position + Value.Length < markdown.Length;
            if (!isNotLast)
                IsOpening = false;

            if (IsOpening)
            {
                if (!tokens.Contains(Type))
                {
                    ShouldBeClosed = true;
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

                if (tokens.Contains(Type))
                {
                    tokenBuilder.Append(markdown[Position]);
                    if (isNotLast)
                        Position--;
                }
                else
                    ShouldBeIgnored = true;
                tokens.Clear();
            }
        }

        public void Validate(string markdown, IEnumerable<IToken> tokens)
        {
            var headings = tokens.Where(t => t.Type == Type);
            if (!IsOpening)
            {
                var openedHeadings = headings.Where(t => t.IsOpening && t.ShouldBeClosed);
                var closedHeadings = headings.Where(t => !t.IsOpening);
                if (openedHeadings.Count() == closedHeadings.Count() + 1)
                    foreach (var openToken in openedHeadings)
                    {
                        var headingToken = (HeadingToken) openToken;
                        headingToken.ShouldBeClosed = false;
                    }
            }
        }
    }
}
