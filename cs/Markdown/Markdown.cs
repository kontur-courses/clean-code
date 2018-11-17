namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new TokenFinder().FindTokensInMdText(paragraph);
            var tokensPositions = new TokensValidator().GetPositionsForTokens(tokens);
            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, tokensPositions);

            return htmlParagraph;
        }
    }
}
