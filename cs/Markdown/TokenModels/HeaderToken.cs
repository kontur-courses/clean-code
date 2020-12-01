using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class HeaderToken : IToken
    {
        public static readonly string MdTag = TagsConfigReader.GetMdTagForTokenName(nameof(HeaderToken));
        private StringToken Children { get; }

        public int MdTokenLength => MdTag.Length + Children.MdTokenLength;

        public HeaderToken(string mdString, int startIndex)
        {
            if (!mdString.StartsWith(MdTag))
                throw new ArgumentException($"{nameof(HeaderToken)} should starts with \"{MdTag}\"");

            var rawStringToken = HtmlConverter.ConvertToHtmlString(mdString.Substring(startIndex + MdTag.Length));
            Children = new StringToken(rawStringToken);
        }

        public string ToHtmlString() => $"<h1>{Children.ToHtmlString()}</h1>";
    }
}