using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class BoldToken : IToken
    {
        private List<IToken> Children { get; }
        public string ToHtmlString() => $"<strong>{Children.ConvertToHtmlString()}</strong>";

        private BoldToken(IEnumerable<IToken> children)
        {
            Children = children.ToList();
        }
        
        public static BoldToken Create(string mdString) => new BoldToken(Tokenizer.ParseIntoTokens(mdString));
    }
}