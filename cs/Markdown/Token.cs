using System;

namespace Markdown
{
    class Token
    {
        public int Index { get; } // Начало токена
        public bool IsTag { get; } // Содержит ли токен тэг
        public int Length { get; } // Длинна токена

        public Token(int index, int length, bool isTag)
        {
            Index = index;
            IsTag = isTag;
            Length = length;
        }
    }
}
