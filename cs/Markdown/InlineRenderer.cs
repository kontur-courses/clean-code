using System.Text;

namespace Markdown
{
    public class InlineRenderer
    {
        private readonly EscapeFinder escapeFinder = new ();
        private readonly UnderscoreFinder underscoreFinder = new ();
        private readonly PairFinder pairFinder = new ();

        public string Render(string text)
        {
            var escapePositions = escapeFinder.FindEscapePositions(text);
            var underscores = underscoreFinder.Find(text, escapePositions);
            var pairs = pairFinder.Find(underscores);

            return CreateHtml(text, pairs, escapePositions);
        }

        private static string CreateHtml(string text, IReadOnlyList<UnderscorePair> pairs, ISet<int> escapePositions)
        {
            var openings = pairs
                .ToDictionary(pair => pair.Opening.Position, pair => pair.Opening);
            var closings = pairs.ToDictionary(pair => pair.Closing.Position, pair => pair.Closing);
            var html = new StringBuilder();
            var position = 0;
            while (position < text.Length)
            {
                if (openings.TryGetValue(position, out var opening))
                {
                    html.Append(HtmlTag.Open(opening.TagName));
                    position += opening.Length;
                    continue;
                }

                if (closings.TryGetValue(position, out var closing))
                {
                    html.Append(HtmlTag.Close(closing.TagName));
                    position += closing.Length;
                    continue;
                }

                if (!escapePositions.Contains(position))
                    html.Append(text[position]);

                position++;
            }

            return html.ToString();
        }
    }
}
