using System;
using System.Text;

namespace Markdown
{
    public class LinkToken : Token
    {
        public readonly int LinkPosition;
        public readonly string Link;

        public LinkToken(int position, int length, Tag tag, TextType type,
            int linkPosition, string link) :
            base(position, length, tag, type)
        {
            LinkPosition = linkPosition;
            Link = link;
        }

        /// <summary>
        /// Метод, который возаращает значение токена, преобразую тип оформления в тег по переданному правилу
        /// </summary>
        /// <param name="tagConverter">Функция, которая преобразует тип текста в обрамляющие тэги</param>
        /// <param name="originalString">Строка, по которой строилсь дерево токенов</param>
        /// <returns>Текст токена, обрамлённый тегами</returns>
        public override string GetValue(Func<Token, Tag> tagConverter, string originalString)
        {
            var tag = tagConverter(this);
            var textLength = (LinkPosition - 2) - (Position + 1);
            var sb = new StringBuilder();
            sb.Append(tag.Open);
            sb.Append(originalString.Substring(Position + 1, textLength));
            sb.Append(tag.Close);
            return sb.ToString();
        }
    }
}