using System.Collections.Generic;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class HeaderToken : IToken
    {
        private IEnumerable<IToken> Children { get; }
        public string ToHtmlString() => $"<h1>{Children.ConvertToHtmlString()}</h1>";
    }
}