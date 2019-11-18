using Markdown.Tokens;
using Markdown.Transducer.ConverterTokenToHtml;

namespace Markdown.Transducer
{
    public interface ITransducer
    {
        void Registred(IToken token, IConverterTokenToHtml converterTokenToHtml);
        string MakeHtmlString(string str, IToken[] tokens);
    }
}