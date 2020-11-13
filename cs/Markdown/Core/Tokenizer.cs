using System.Collections.Generic;
using System.Linq;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class StringExtension
    {
        public static readonly char[] SpecialChars = {'_', '#'};
        
        public static IEnumerable<IToken> ParseIntoTokens(this string mdText)
        {
            var tokens = new List<IToken>();
            var currentField = "";
            for (var i = 0; i < mdText.Length; i++)
            {
                if (SpecialChars.Contains(mdText[i]) && currentField != "")
                {
                    tokens.Add(new StringToken(currentField));
                    currentField = "";
                }
                
                switch (mdText[i])
                {
                    case '_' when mdText[i + 1] == '_':
                    {
                        i += 2;
                        var startIndex = i;
                        while (mdText[i].ToString() + mdText[i + 1] != "__") i++;
                        
                        var mdString = mdText.Substring(startIndex, i++ - startIndex);
                        var boldToken = BoldToken.Create(mdString);
                        
                        tokens.Add(boldToken);
                        break;
                    }
                    case '_':
                    {
                        i += 1;
                        var startIndex = i;
                        while (mdText[i] != '_') i++;
                        
                        var mdString = mdText.Substring(startIndex, i - startIndex);
                        var italicToken = ItalicToken.Create(mdString);
                        
                        tokens.Add(italicToken);
                        break;
                    }
                    default:
                        currentField += mdText[i];
                        break;
                }
            }
            
            if (currentField != "") tokens.Add(new StringToken(currentField));

            return tokens;
        }
    }
}