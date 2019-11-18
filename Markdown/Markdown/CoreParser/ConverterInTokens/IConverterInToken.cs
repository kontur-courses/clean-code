using Markdown.Tokens;

namespace Markdown.ConverterInTokens
{
    public interface IConverterInToken
    {
        void RegisterNested(IConverterInToken converterInToken);
        IToken MakeConverter(string text, int startIndex);
    }
}