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
         * Исходя из текшего символа и символов в буфере записывает с stringBilder символы для дальнейшего превращения в конечную строку.
        */
        public void Handler(char symbol)
        {
            throw new NotImplementedException();
        }
    }
}
