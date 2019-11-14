using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        private TextType outerTokenType;

        private TextType tokenType;

        private Dictionary<int, Token> innerTokensDict;


        public Token(string text, TextType outerType)
        {
            outerTokenType = outerType;            
            SplitTextToTokens(text);         
        }

        private void SplitTextToTokens(string text)
        {
            //делим на токены, помещаем в innerTokensDict
            //ключ - позиция в первоначальном тексте

        }

        private string ToAnotherMarkupLanguage()
        {
            /*
            foreach (KeyValuePair<string, string> entry in innerTokensDict)
            {
                используем HTMLTranslator.ToHTML() для каждого внутреннего токена, соединяем их вместе.          
            }
            */
            throw new NotImplementedException();
        }
    }
}
