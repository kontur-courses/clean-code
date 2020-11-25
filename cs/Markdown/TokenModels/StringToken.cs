using Markdown.Core;

namespace Markdown.TokenModels
{
    public class StringToken : IToken
    {
        public string MdTag => "";
        public int MdTokenLength => Value.Length;
        private string Value { get; }

        private StringToken(string value) => Value = value;

        public static StringToken Create(string value) => new StringToken(value);
        public string ToHtmlString() => Value;
    }
}