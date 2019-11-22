using Markdown.Tokens;

namespace Markdown.Transducer.ConverterTokenToHtml
{
    public class AbstractConverterTokenToHtml : IConverterTokenToHtml
    {
        private Transducer transducer = new Transducer();

        public AbstractConverterTokenToHtml(string startString, string endString)
        {
            StartString = startString;
            EndString = endString;
        }

        public void RegisterNested(IToken token, IConverterTokenToHtml converterInToken)
        {
            transducer.Registred(token, converterInToken);
        }
        
        public string StartString { get; }
        public string EndString { get; }
        
        public string MakeConverter(IToken token)
        {
            if (token.NestedTokens.Length == 0)
                return StartString + token.Text + EndString;
            return StartString + transducer.MakeHtmlString(token.Text, token.NestedTokens) + EndString;
        }
    }
}