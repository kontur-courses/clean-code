using Markdown.Tokens;

namespace Markdown.CoreParser.ConverterInTokens
{
    public interface IConverterInToken
    {
        void RegisterNested(IConverterInToken converterInToken);
        IToken SelectTokenInString(string text, int startIndex);
    }
}