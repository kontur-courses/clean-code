using Markdown.Core;

namespace Markdown.TokenModels
{
    public class StringToken : IToken
    {
        private string Value { get; }
        
        public StringToken(string value) => Value = value;
        public string ToHtmlString() => Value;
    }
}