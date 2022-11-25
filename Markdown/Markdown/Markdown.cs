namespace Markdown
{
    public class Markdown
    {
        public static string Render(string text)
        {
            var line = text.Split('\n', '\r');
            var convertedLines = new List<string>();
            var markdown = new Markdown();
            for (int i = 0; i < line.Length; i++)
            {
                convertedLines.Add(RenderLine(line[i]));
            }

            return string.Join('\n', convertedLines);
        }

        private static string RenderLine(string line)
        {
            var stringFormat = SpecialStringFormat.ConvertLineToFormat(line)
                .SetPrimaryMarkdown()
                .DisapproveIntersectingPairs()
                .DisapproveEmpty()
                .DisapproveStartsOrEndsWithSpace();

            return stringFormat.ConvertFromFormat();
        }
    }
}