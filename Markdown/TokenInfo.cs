namespace Markdown
{
    public record TokenInfo(Token Token, bool CloseValid, bool OpenValid, bool WordPartPlaced, bool Valid);
}