using System.Collections.Generic;

namespace Markdown
{
    public class TagLevelTokenizer
    {
        /*Создание нового списка с токенами второго уровня: обрабатка токенов первого уровня и замена их на
         SecondTokenType, 
         получился новый список с Токенами, в котором лежат токены, которые потенциально могут быть тегами
        */
        public List<SecondLevelToken> Tokenize(List<FirstTokenType> characterTokenList)
        {
            return new List<SecondLevelToken>();
        }
    }
}