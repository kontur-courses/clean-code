using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    static class TokenWriter
    {
        public static string Write(Token token, string line)
        {
            return null;
            // Обходим дерево токенов слева направо, попутно изменяя line
            // Делаем поправку на индексы с учетом того, что мы удлиняем строку, заменяя свои теги на html теги
            // Возвращаем измененную строку
        }

        // Почти уверен, что здесь тоже будут вспомогательные методы
    }
}
