using Markdown.Tokens;

namespace Markdown.Transducer.ConverterTokenToHtml
{
    public interface IConverterTokenToHtml
    {
        void RegisterNested(IToken token, IConverterTokenToHtml converterInToken);
        string MakeConverter(IToken token);
    }
}