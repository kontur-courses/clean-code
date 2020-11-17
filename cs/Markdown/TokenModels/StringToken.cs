using Markdown.Core;

namespace Markdown.TokenModels
{
    public class StringToken : IToken
    {
        private string Value { get; }
        public int MdTokenLength => Value.Length;
        
        private StringToken(string value) => Value = value;
        
        public static StringToken Create(string value) => new StringToken(value);
        public string ToHtmlString() => Value;
    }
}