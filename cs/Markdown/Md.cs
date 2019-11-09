using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Md
    {
        /*
         * Получает на вход текст в формате Markdown, проходит по каждому символу и передаёт его в метод Handler
         */
        public string Render(string text)
        {
            throw new NotImplementedException();
        }

        /*
         * С помошью switch определяет специальные символы и при необходимости записывает их в буфер
         * для сопоставления со следующими символами.
         * Исходя из текшего символа и символов в буфере создаёт токен соответствующий текущей части текста, который клаётся в массив.
        */
        private Token[] ParseStringToTokens(string text)
        {
            throw new NotImplementedException();
        }

        /*
        * Проходит по массиву токенов и переводит их в формат html.
        */
        private string ParseTokensToHtml(Token[] tokens)
        {
            throw new NotImplementedException();
        }
    }
}
