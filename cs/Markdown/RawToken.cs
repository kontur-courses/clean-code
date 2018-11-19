using Markdown.Markups;

namespace Markdown
{
    public class RawToken
    {
        public readonly int OpeningPosition;
        public readonly int ClosingPosition;
        public readonly Markup Markup;

        public RawToken(int openingPosition, int closingPosition, Markup markup)
        {
            OpeningPosition = openingPosition;
            ClosingPosition = closingPosition;
            Markup = markup;
        }

        public bool Empty()
        {
            return ClosingPosition - OpeningPosition <= 1;
        }
    }
}
