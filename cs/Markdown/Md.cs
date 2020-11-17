namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var italicTagInfo = new EmphasizingTagInfo("_", "em");
            var boldTagInfo = new EmphasizingTagInfo("__", "strong");
            var headerTagInfo = new SingleTagInfo("# ", "h1", "\n");
            var linkTagInfo = new LinkTagInfo("[", "a");
            var tokens =
                TokenParser.ParseStringToMdTokens(text, italicTagInfo, boldTagInfo, headerTagInfo, linkTagInfo);
            var textWithAlliedTokens = TokenApplier.ApplyTokensToString(text, tokens);
            return TokenParser.ScreenSymbols(textWithAlliedTokens, italicTagInfo, boldTagInfo, headerTagInfo,
                linkTagInfo);
        }
    }
}