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
            var descriptionStart = startIndex + "[".Length;
            var descriptionEndIndex = mdString.IndexOf(']', descriptionStart);
            TokenThrowHelper.AssertDescriptionIsCorrect(mdString, startIndex, descriptionEndIndex);

            var linkStart = descriptionEndIndex + 1 + "(".Length;
            var linkEndIndex = mdString.IndexOf(')', linkStart);
            TokenThrowHelper.AssertLinkIsCorrect(mdString, linkEndIndex, descriptionEndIndex);

            var descriptionLength = descriptionEndIndex - descriptionStart;
            var description = mdString.Substring(descriptionStart, descriptionLength);
            
            var linkLength = linkEndIndex - linkStart;
            var link = mdString.Substring(linkStart, linkLength);
            return new LinkToken(description, link);
        }
    }
}