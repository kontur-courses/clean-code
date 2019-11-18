using Markdown.ConverterInTokens;
using Markdown.ConverterTokens;
using Markdown.Tokens;

namespace Markdown.CoreParser
{
    public interface IParser
    {
        void register(IConverterInToken reader);
        IToken[] tokenize(string str);
    }
}