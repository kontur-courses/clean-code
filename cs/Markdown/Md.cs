namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var italicTagInfo = new TagInfo("_", "em", false);
            var boldTagInfo = new TagInfo("__", "strong", false);
            var headerTagInfo = new TagInfo("#", "h1", true);
            var tokens = TokenParser.ParseStringToMdTokens(text, italicTagInfo, boldTagInfo, headerTagInfo);
            return TokenApplier.ApplyTokensToString(text, tokens);
        }
    }
}