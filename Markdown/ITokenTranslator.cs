namespace Markdown
{
    public interface ITokenTranslator
    {
        void SetTranslateRule(Token from, Token to);
        Token Translate(Token token);
    }
}