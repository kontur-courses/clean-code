using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public int Index { get; } // Начало токена
        public bool IsTag { get; } // Содержит ли токен тэг
        public int Length { get; } // Длинна токена

        public Token(int index, int length, bool isTag)
        {
            throw new NotImplementedException();
        }
    }
}
