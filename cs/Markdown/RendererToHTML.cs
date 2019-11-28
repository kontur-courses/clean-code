using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class RendererToHTML
    {
        public string ToHTML(string text, List<Token> tokens)
        {
            var shift = 0;
            var aStringBuilder = new StringBuilder(text);
            var mainTokens = tokens.Where(x => x.outerToken == null);
            var innerTokens = tokens.Where(x => x.outerToken != null);          
            var mainTokensDict = new Dictionary<int, int>();
            foreach (var token in mainTokens)
            {
                var additionalShift = (token.tokenTextType == TextType.Italic) ? 3 : 6;
                var isStrong = (additionalShift == 3) ? false : true;
                mainTokensDict.Add(token.startTagPosition, shift + additionalShift);
                shift = TransformToken(aStringBuilder, token, shift, false, isStrong, text);                
            }
            shift = 0;
            var currentSuperiorTokenLocation = -1;
            foreach (var token in innerTokens)
            {
                if (currentSuperiorTokenLocation != token.outerToken.Item2)
                {
                    currentSuperiorTokenLocation = token.outerToken.Item2;
                    shift += mainTokensDict[token.outerToken.Item2];
                }                
                var isBold = (token.tokenTextType== TextType.Strong) ? true : false;                
                shift = TransformToken(aStringBuilder, token, shift, true, isBold, text);           
            }            
            var finalString = aStringBuilder.ToString();
            return finalString.Replace(@"\", "");

        }        
        
        private int TransformToken(StringBuilder aStringBuilder, Token token, int shift, bool isInside, bool isStrong, string text)
        {
            if (CheckForSurroundingNumbers(token, text))
                return 0;
            var tags = new Tuple<string, string>("<em>", "</em>");
            var coef = 1;
            if (isStrong)
            {
                tags = new Tuple<string, string>("<strong>", "</strong>");
                coef = 2;
            }
            if (isStrong && isInside)
                tags = new Tuple<string, string>("", "");
            aStringBuilder.Remove(token.startTagPosition + shift, coef);
            aStringBuilder.Insert(token.startTagPosition + shift, tags.Item1);
            shift += tags.Item1.Length - coef;            
            aStringBuilder.Remove(token.closeTagPosition + shift, coef);
            aStringBuilder.Insert(token.closeTagPosition + shift, tags.Item2);
            shift += tags.Item2.Length - coef - 1;
            return shift + 1;
        }

        private bool CheckForSurroundingNumbers(Token token, string text)
        {
            var isSurroundedByNumbers = false;
            var coef = 1;
            if (token.tokenTextType == TextType.Strong)
                coef = 2;
            if (token.startTagPosition > 0)
            {
                if (char.IsDigit(text[token.startTagPosition - 1]) && char.IsDigit(text[token.startTagPosition + coef]))
                    isSurroundedByNumbers = true;
            }
            if (token.closeTagPosition < text.Length - coef)
            {
                if (char.IsDigit(text[token.closeTagPosition - 1]) && char.IsDigit(text[token.closeTagPosition + coef]))
                    isSurroundedByNumbers = true;
            }
            return isSurroundedByNumbers;
        }                  
    }
}
