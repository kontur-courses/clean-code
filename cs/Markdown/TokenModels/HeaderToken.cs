using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class HeaderToken : IToken
    {
        public static string MdTag => "# ";
        private StringToken Children { get; }

        public int MdTokenLength => MdTag.Length + Children.MdTokenLength;

        private HeaderToken(StringToken children) => Children = children;

        public string ToHtmlString() => $"<h1>{Children.ToHtmlString()}</h1>";

        public static HeaderToken Create(string mdString, int startIndex)
        {
            if (!mdString.StartsWith(MdTag))
                throw new ArgumentException($"{nameof(HeaderToken)} should starts with \"{MdTag}\"");

            var childrenSource = HtmlConverter.ConvertToHtmlString(mdString.Substring(startIndex + MdTag.Length));
            return new HeaderToken(StringToken.Create(childrenSource));
        }
    }
}