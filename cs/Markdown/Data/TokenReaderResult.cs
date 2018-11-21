namespace Markdown.Data
{
    public class TokenReaderResult
    {
        public readonly bool Success;
        public readonly int Shift;
        public readonly Token Token;

        public TokenReaderResult(bool success, int shift = 0, Token token = null)
        {
            Success = success;
            Shift = shift;
            Token = token;
        }
    }
}