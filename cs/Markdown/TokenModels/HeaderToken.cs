using System.Collections.Generic;
using System.Linq;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class HeaderToken : IToken
    {
        private IEnumerable<IToken> Children { get; }

        private HeaderToken(IEnumerable<IToken> children) => Children = children;

        public int MdTokenLength => Children.Sum(t => t.MdTokenLength);
        public string ToHtmlString() => $"<h1>{Children.ConvertToHtmlString()}</h1>";

        public static HeaderToken Create(string mdString) =>
            new HeaderToken(Tokenizer.ParseIntoTokens(mdString.Substring(2)));
    }
}