using Markdown.ConverterTokens;
using Markdown.CoreParser;
using Markdown.Tokens;

namespace Markdown.ConverterInTokens
{
    public abstract class AbstractConverterInToken : IConverterInToken
    {
        private IParser parser = new CoreParser.Parser();
        public void RegisterNested(IConverterInToken converterInToken) => parser.register(converterInToken);
        private string startToken;
        private string endToken;

        protected AbstractConverterInToken(string startToken, string endToken)
        {
            this.startToken = startToken;
            this.endToken = endToken;
        }

        public abstract IToken GetCurrentToken(string Text, int startIndex, IToken[] nestedTokens);
        public IToken MakeConverter(string text, int startIndex)
        {
            if (text.Length - 1 < startIndex + startToken.Length + 1 ||  !text.Substring(startIndex).StartsWith(startToken))
                return null;
            var insideWord = !(startIndex == 0 || text[startIndex - 1].IsSeparatorOrPunctuation());
            var haveSpace = false;
            var haveDigit = false;
            if (text[startIndex + startToken.Length] == ' ')
                return null;
            for (var i = startIndex + startToken.Length; i < text.Length; i++)
            {
                haveSpace = haveSpace || text[i] == ' ';
                haveDigit = haveDigit || char.IsDigit(text[i]);

                if ((haveSpace || haveDigit) && insideWord )
                    return null;
                    
                if (text[i] == '\\')
                {
                    i++;
                    continue;
                }

                if (!text.Substring(i).StartsWith(endToken) || text[i - 1] == ' ') continue;
                
                if (( i == text.Length - endToken.Length || text[i + endToken.Length].IsSeparatorOrPunctuation() || insideWord))
                {
                    //знаю, что костыль и надо отнего избавиться, но пока не знаю как
                    if (text.Substring(i + endToken.Length).StartsWith(endToken))
                    {
                        i += endToken.Length * 2;
                        continue;
                    }
                    
                    if (i - startIndex - endToken.Length == 0) return null;
                    var newText = text.Substring(startIndex + startToken.Length, i - startIndex - endToken.Length);
                    return GetCurrentToken(newText, startIndex, parser.tokenize(newText));
                }
            }
            return null;
        }
        
    }
    

    public static class CharExtenstion
    {
        public static bool IsSeparatorOrPunctuation(this char c)
        {
            return char.IsPunctuation(c) || char.IsSeparator(c);
        }
    }
}