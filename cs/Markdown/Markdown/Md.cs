namespace Markdown
{
    class Md
    {
        public static string Render(string markedParagraph)
        {
            var parser = new Parser();
            var paragraphParts = parser.Parse(markedParagraph);
            var concatenator = new Concatenator();

            return concatenator.Concatenate(paragraphParts);
        }
    }
}
