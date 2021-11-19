namespace Markdown
{
    interface ITag : IToken
    {
        bool IsClosed
        { get; set; }

        bool IsStartTag
        { get; set; }

        bool IsAtTheBeginning
        { get; set; }
    }
}
