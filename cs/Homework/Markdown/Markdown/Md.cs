namespace Markdown
{
    public class Md
    {
        public string Render(string mdParagraph)
        {
            var tokens = MdParser.GetAllTokens(mdParagraph);
            tokens = MdParser.GetNotIntersectingEndsTokens(tokens);
            tokens = MdParser.RemoveNotWorkingNestedTokens(tokens);

            return new HtmlWriter(mdParagraph, tokens).GetHtmlParagraph();
        }
    }
}
