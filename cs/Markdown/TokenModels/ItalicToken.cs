using System.Collections.Generic;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        private IEnumerable<IToken> Children { get; }
        public string ToHtmlString() => $"<em>{Children.ConvertToHtmlString()}</em>";
    }
}