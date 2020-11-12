namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var italicTagInfo = new TagInfo("_", "<em>", "</em>");
            var boldTagInfo = new TagInfo("__", "<strong>", "</strong>");
            var headerTagInfo = new TagInfo("#", "<h1>", "</h1>");
            var tokens = Token.ParseStringToMdTokens(text, italicTagInfo, boldTagInfo, headerTagInfo);
            return Token.ApplyTokensToString(text, tokens);
        }
    }
}