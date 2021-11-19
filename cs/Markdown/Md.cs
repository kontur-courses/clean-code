namespace Markdown
{
    public class Md
    {
        public string Render(string rawInput)
        {
            var parsedInput = MarkdownParser.Parse(rawInput);
            return "";
        }
    }
}
