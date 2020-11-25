using Markdown.Core;

namespace Markdown.TokenModels
{
    public class LinkToken : IToken
    {
        public static string MdTag => "[";
        public int MdTokenLength => "[".Length + Link.MdTokenLength + "]".Length +
                                    "(".Length + Description.MdTokenLength + ")".Length;

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
            TokenThrowHelper.AssertDescriptionIsCorrect(mdString, startIndex, descriptionEndIndex);

            var linkEndIndex = mdString.IndexOf(')', descriptionEndIndex + 1);
            TokenThrowHelper.AssertLinkIsCorrect(mdString, linkEndIndex, descriptionEndIndex);

            var description = mdString.Substring(startIndex + 1, descriptionEndIndex - startIndex - 1);
            var link = mdString.Substring(descriptionEndIndex + 2, linkEndIndex - descriptionEndIndex - 2);
            return new LinkToken(description, link);
        }
    }
}