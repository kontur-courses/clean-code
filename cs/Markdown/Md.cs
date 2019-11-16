using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Md
    {
        private Dictionary<string, Tag> TagSymbols;

        /*
         * Получает на вход текст в формате Markdown, создаёт экземпляр парсера и передаёт их методу ConvertTextWithParser
         */
        public string Render(string text)
        {
            throw new NotImplementedException();
        }        

        /*
         * Проверяет есть ли символ в словаре и если есть то действует по предписаниям тэга: продолжает собирать токен или начинает новый
         * Если символа нет в словате считает это токеном с каким-то текстом
         */
        private string ConvertTokensToHtml(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        /*
         * проходит по полученым токенам и переводит их в html по предписаниям из тэга
         */
        private List<Token> ParseTextToTokens(string text)
        {
            throw new NotImplementedException();
        }
    }
}
