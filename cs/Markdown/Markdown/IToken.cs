namespace Markdown
{
    public interface IToken
    {
        string Content
        { get;}
        bool IsPrevent
        { get; set; }
    }
}
