using System.Collections.Generic;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class HeaderToken : IToken
    {
        private IEnumerable<IToken> Children { get; }
        
        private HeaderToken(IEnumerable<IToken> children) => Children = children;
        
        public string ToHtmlString() => $"<h1>{Children.ConvertToHtmlString()}</h1>\n";
        public static HeaderToken Create(string mdString) => new HeaderToken(Tokenizer.ParseIntoTokens(mdString.TrimStart()));
    }
}