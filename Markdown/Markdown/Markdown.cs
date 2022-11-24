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
                convertedLines.Add(MarkdownRenderer.RenderLine(line[i]));
            }

            return string.Join('\n', convertedLines);
        }
    }
}