namespace Markdown.RenderUtilities
{
    public class PairedTokenDescription
    {
        public readonly Token OpenToken;
        public readonly Token CloseToken;

        public PairedTokenDescription(Token openToken, Token closeToken)
        {
            OpenToken = openToken;
            CloseToken = closeToken;
        }
    }
}
