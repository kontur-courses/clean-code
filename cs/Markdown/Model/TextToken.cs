using Markdown.Interfaces;

namespace Markdown.Model
{
    public class TextToken : IToken
    {
        public string Value { get; set; }

        public bool IsOpenTag { get; set; }

        public string ConvertToHtml()
        {
            return Value;
        }
    }
}