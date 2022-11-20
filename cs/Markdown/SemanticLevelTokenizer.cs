using System.Collections.Generic;

namespace Markdown
{
    public class SemanticLevelTokenizer
    {
        /*Анализ предыдущего списка: опредление может ли токен быть тегом или нет, если нет, то тогда его тип
         заменяется на String*/
        public List<SecondLevelToken> Tokenize(List<SecondLevelToken> tagTokenList)
        {
            return new List<SecondLevelToken>();
        }
    }
}