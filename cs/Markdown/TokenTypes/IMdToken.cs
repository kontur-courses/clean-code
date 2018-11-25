namespace Markdown.TokenTypes
{
    public interface IMdToken
    {
        string Delimiter { get; set; }
        string HtmlTag { get; set; }
        bool IsPair { get; set; }
    }
}