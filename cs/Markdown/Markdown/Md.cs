namespace Markdown
{
    class Md
    {
        public static string Render(string markedParagraph, char escapeSymbol = '/')
        {
            var paragraphParts = Parser.Parse(markedParagraph, escapeSymbol);
            var concatenator = new Concatenator();

            return concatenator.Concatenate(paragraphParts);
        }
    }
}
