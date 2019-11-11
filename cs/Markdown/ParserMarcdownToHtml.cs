using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class ParserMarcdownToHtml : IParser
    {
        private Dictionary<char, Tag> TagSymbols;

        public ParserMarcdownToHtml()
        {
            TagSymbols = new Dictionary<char, Tag>();
        }

        /*
         * Проверяет есть ли символ в словаре и если есть то действует по предписаниям тэга: продолжает собирать токен или начинает новый
         * Если символа нет в словате считает это токеном с каким-то текстом
         */
        public string ConvertTokensToHtml(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        /*
         * проходит по полученым токенам и переводит их в html по предписаниям из тэга
         */
        public List<Token> ParseTextToTokens(string text)
        {
            throw new NotImplementedException();
        }
    }
}
