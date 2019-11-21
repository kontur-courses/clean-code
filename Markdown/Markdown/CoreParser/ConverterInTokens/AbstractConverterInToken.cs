using System.Linq;
using Markdown.Tokens;

namespace Markdown.CoreParser.ConverterInTokens
{
    public abstract class AbstractConverterInToken : IConverterInToken
    {
        private readonly IParser parser;
        private readonly string startToken;
        private readonly string endToken;
        private readonly char[] unacceptableCharsAfterEndToken;


        protected AbstractConverterInToken(string startToken, string endToken) : this(startToken, endToken, new char[0])
        {
        }

        protected AbstractConverterInToken(string startToken, string endToken, char[] unacceptableCharsAfterEndToken)
        {
            parser = new Parser();
            this.startToken = startToken;
            this.endToken = endToken;
            this.unacceptableCharsAfterEndToken = unacceptableCharsAfterEndToken;
        }
        
        public void RegisterNested(IConverterInToken converterInToken) => parser.Register(converterInToken);

        protected abstract IToken GetCurrentToken(string text, int startIndex, IToken[] nestedTokens);

        public IToken SelectTokenInString(string text, int startIndex)
        {
            var haveSpace = false;
            var haveDigit = false;
            if (!InitialCheckForCorrectness(text, startIndex, out var insideWord))
                return null;
            
            for (var i = startIndex + startToken.Length; i < text.Length; i++)
            {
                haveSpace = haveSpace || text[i] == ' ';
                haveDigit = haveDigit || char.IsDigit(text[i]);
                
                if ((haveSpace || haveDigit) && insideWord )
                    return null;
                
                if (text[i] == '\\' )
                {
                    i++;
                    continue;
                }

                if (CheckForFinalSubstring(text, ref i, insideWord))
                {
                    var newText = text.Substring(startIndex + startToken.Length, i - startIndex - endToken.Length);
                    return GetCurrentToken(newText, startIndex, parser.Tokenize(newText));
                }
            }
            return null;
        }
        
        private bool InitialCheckForCorrectness(string text, int startIndex, out bool insideWord)
        {
            insideWord = !(startIndex == 0 || text[startIndex - 1].IsSeparatorOrPunctuation());

            return text.Length - startIndex >=  startToken.Length + endToken.Length + 1 &&
                   text.Substring(startIndex).StartsWith(startToken) &&
                   text[startIndex + startToken.Length] != ' ';
        }
        
        private bool CheckForFinalSubstring(string text, ref int i, bool insideWord)
        {
            return text[i - 1] != ' ' && text.Substring(i).StartsWith(endToken) &&
                   (i == text.Length - endToken.Length || text[i + endToken.Length].IsSeparatorOrPunctuation() ||
                    insideWord) && !CheckForSkipCharactersAfterEndOfTheToken(text, ref i);
        }
        
        private bool CheckForSkipCharactersAfterEndOfTheToken(string text, ref int i)
        {
            if (i + endToken.Length >= text.Length || !unacceptableCharsAfterEndToken.Contains(text[i + endToken.Length])) return false;
            i += endToken.Length + 1;
            while (i < text.Length && endToken.Contains(text[i])) i++;
            return true;
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