using Markdown.Core;

namespace Markdown.TokenModels
{
    public class HeaderToken : IToken
    {
        private StringToken Children { get; }

        private HeaderToken(StringToken children) => Children = children;

        public int MdTokenLength => "# ".Length + Children.MdTokenLength;
        public string ToHtmlString() => $"<h1>{Children.ToHtmlString()}</h1>";

        public static HeaderToken Create(string mdString) =>
            new HeaderToken(StringToken.Create(HtmlConverter.ConvertToHtmlString(mdString.Substring(2))));
    }
}