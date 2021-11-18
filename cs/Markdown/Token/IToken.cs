namespace Markdown.Token
{
    public interface IToken
    {
        int Start { get; set; }
        int Length { get; set; }
        ITag Tag { get; set; }
    }
}