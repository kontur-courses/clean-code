using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    /// <summary>
    /// Хранит в данные об участке текста, который оформляется одним способом
    /// </summary>
    public class Token
    {
        public readonly int Length;
        public readonly int Position;
        public readonly TextType Type;
        public readonly Tag Tag;

        public int NextIndex => Position + Length;

        private readonly List<Token> internalTokens;

        public Token(int position, int length, Tag tag, TextType type)
        {
            Length = length;
            Position = position;
            Type = type;
            Tag = tag;
            internalTokens = new List<Token>();
        }

        /// <summary>
        /// Метод, который возаращает значение токена, преобразую тип оформления в тэк по переданному правилу
        /// </summary>
        /// <param name="tagConverter">Функция, которая преобразует тип текста в обрамляющие тэги</param>
        /// <param name="originalString">Строка, по которой строилсь дерево токенов</param>
        /// <returns>Текст токена, обрамлённый тегами</returns>
        public string GetValue(Func<TextType, Tag> tagConverter, string originalString)
        {
            var builder = new StringBuilder();
            var newTag = tagConverter(Type);
            var startIndex = Position + Tag.Open.Length;
            
            builder.Append(newTag.Open);
            foreach (var token in internalTokens.OrderBy(token => token.Position))
            {
                builder.Append(originalString.Substring(startIndex, token.Position - startIndex));
                builder.Append(token.GetValue(tagConverter, originalString));
                startIndex = token.NextIndex;
            }
            builder.Append(originalString.Substring(startIndex, Length - Tag.Close.Length - (startIndex - Position)));
            builder.Append(newTag.Close);
            
            return builder.ToString();
        }

        public void AddInternalToken(Token token)
        {
            internalTokens.Add(token);
        }

        public bool RemoveInternalToken(Token token)
        {
            return internalTokens.Remove(token);
        }
    }
}