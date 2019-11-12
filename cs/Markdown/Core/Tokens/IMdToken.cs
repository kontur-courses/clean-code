namespace Markdown
{
    public interface IMdToken
    {
        int Position { get; }
        int Length { get; }
        string Value { get; }
        MdTokenType TokenType { get; set; }
    }
}