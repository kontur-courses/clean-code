using Markdown.CoreParser.ConverterInTokens;
using Markdown.Tokens;

namespace Markdown.CoreParser
{
    public interface IParser
    {
        void Register(IConverterInToken reader);
        IToken[] Tokenize(string str);
    }
}