using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    /// <summary>
    /// Хранит в себе кусок текста, который оформляется одним способом
    /// </summary>
    public class Token
    {
        public readonly int Length; // Длина всего участка строки
        public readonly int Position; // Позиция в исходной строке
        public readonly TextType Type; // Тип текста, в который нужно оформить
        public readonly int TagLength; // Длина тэга форматирования
        public int NextIndex { get; } // Индекс исходной строки, следующий после конца токена

        private readonly List<Token> internalTokens; // Список потомков в дереве токенов
        
        public Token(int length, int position, TextType type, int tagLength)
        {
            Length = length;
            Position = position;
            Type = type;
            TagLength = tagLength;
            internalTokens = new List<Token>();
        }

        /// <summary>
        /// Метод, который возаращает значение токена, преобразую тип оформления в тэк по переданному правилу
        /// </summary>
        /// <param name="tagConverter">Функция, которая преобразует тип текста в обрамляющие тэги</param>
        /// <returns>Текст токена, обрамлённый тегами</returns>
        public string GetValue(Func<TextType, Tag> tagConverter)
        {
            throw new NotImplementedException();
        }

        public void AddInternalToken(Token token)
        {
            internalTokens.Add(token);
        }
    }
}