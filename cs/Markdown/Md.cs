using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Md
    {
        public string Render(string input)
        {
            // Сначала создаем основной Token, с данными параметрами:
            // StartPosition = 0
            // Length = input.Length
            // SubTokens = null;
            // TokenType = TokenType.Simple
            // Таким образом мы с самого начала представляем весь наш текст в виде одного большого токена обычного типа 
            // Затем для этого токена вызываем TokenReader.Read
            // Затем получаем финальную строку, передав этот токен в TokenWriter.Write
            throw new NotImplementedException();
        }
    }
}