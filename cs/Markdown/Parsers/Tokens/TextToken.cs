using System.Linq;
using Markdown.Parsers.Tokens.Html;
using Markdown.Parsers.Tokens.Markdown;

namespace Markdown.Parsers.Tokens
{
    public class TextToken : Token
    {
        public TextToken(string data) : base(data)
        {
        }

        public override IToken ToHtml() => new HtmlTextToken(text);

        public override IToken ToMarkdown() => new MdTextToken(text);

        public bool IsWord() => !text.ToCharArray().Any(c => char.IsWhiteSpace(c) || char.IsDigit(c));
        
    }
}
