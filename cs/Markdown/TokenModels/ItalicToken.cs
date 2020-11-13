using System.Collections.Generic;
using System.Linq;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        private List<IToken> Children { get; }
        
        private ItalicToken(IEnumerable<IToken> children) => Children = children.ToList();
        
        public string ToHtmlString() => $"<em>{Children.ConvertToHtmlString()}</em>";

        public static ItalicToken Create(string mdString) => new ItalicToken(Tokenizer.ParseIntoTokens(mdString));
    }
}