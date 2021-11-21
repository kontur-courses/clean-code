namespace Markdown
{
    public record TokenInfo(int Position, Token Token, bool CloseValid, bool OpenValid, bool WordPartPlaced, bool Valid);
}