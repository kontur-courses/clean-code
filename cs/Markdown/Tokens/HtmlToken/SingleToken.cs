using Markdown.Tokens.HtmlToken;

namespace Markdown.Tokens;

public  class SingleToken : BaseHtmlToken
{
    public SingleToken(string value) : base(null)
    {
        Value = value;
    }
    public override string Value { get; }
}