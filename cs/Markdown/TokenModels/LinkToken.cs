using Markdown.Core;

namespace Markdown.TokenModels
{
    public class LinkToken : IToken
    {
        public int MdTokenLength => 1 + Link.MdTokenLength + 1 + 1 + Description.MdTokenLength + 1;
        private StringToken Link { get; }
        private StringToken Description { get; }

        private LinkToken(string description, string link)
        {
            Description = StringToken.Create(description);
            Link = StringToken.Create(link);
        }

        public string ToHtmlString() => $"<a href=\"{Link.ToHtmlString()}\">{Description.ToHtmlString()}</a>";

        public static LinkToken Create(string mdString, int startIndex)
        {
            var descriptionEndIndex = mdString.IndexOf(']', startIndex + 1);
            if (descriptionEndIndex - startIndex <= 1 || IsNotParenthesis(mdString, descriptionEndIndex + 1))
                return default;
            var linkEndIndex = mdString.IndexOf(')', descriptionEndIndex + 1);
            if (linkEndIndex - (descriptionEndIndex + 1) <= 1 || mdString[linkEndIndex] != ')')
                return default;

            var description = mdString.Substring(startIndex + 1, descriptionEndIndex - startIndex - 1);
            var link = mdString.Substring(descriptionEndIndex + 2, linkEndIndex - descriptionEndIndex - 2);
            return new LinkToken(description, link);
        }

        private static bool IsNotParenthesis(string mdString, int index) =>
            mdString.IsCharInsideString(index) && mdString[index] != '(';
    }
}