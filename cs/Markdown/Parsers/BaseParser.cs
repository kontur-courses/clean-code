namespace Markdown.Parsers
{
    public abstract class BaseParser : IParser
    {
        // ReSharper disable once InconsistentNaming
        protected readonly Tag tag;

        protected BaseParser(Tag tag)
        {
            this.tag = tag;
        }

        /// <summary>
        /// Пытается обработать участок строки, начинающийся с командного символа
        /// </summary>
        /// <param name="position">Позиция, на которой находится командный символ</param>
        /// <param name="text">Строка, в которой происходит парсинг</param>
        /// <returns>Token, если удалось обработать тэг; null, если обработчик его не поддерживает</returns>
        public abstract Token TryHandleTag(int position, string text);
        
        /// <summary>
        /// Метод, который является подобием ДКА и помогает следить за состоянием поиска закрывающего тега
        /// </summary>
        /// <param name="currentChar">Символ в строке, который нужно проверить</param>
        /// <param name="currentState">Текущее состояние ДКА</param>
        /// <returns>Новое состояние ДКА</returns>
        protected int GetState(char currentChar, int currentState)
        {
            if (currentState >= tag.Close.Length)
                return 0;
            if (currentChar != tag.Close[currentState])
                return 0;
            return currentState + 1;
        }
        
        protected bool IsEmptySelection(int start, int end)
        {
            return tag.Open.Length + tag.Close.Length == end - start + 1;
        }

        protected bool IsOpenInWord(int position, string text)
        {
            return position != 0 && char.IsLetter(text[position-1]);
        }

        protected bool NextCharAfterTag_IsSpaceOrEndLine(int position, string text, string currentTag)
        {
            if (position + currentTag.Length >= text.Length)
                return true;
            return text[position + currentTag.Length] == ' '
                   || text[position + currentTag.Length] == '\n';
        }

        protected bool IsIntoNumber(int start, int end, string text)
        {
            if (start > 0)
                if (char.IsDigit(text[start - 1]) && char.IsDigit(text[start + tag.Open.Length]))
                    return true;
            if (end >= text.Length - 1) return false;
            return char.IsDigit(text[end + 1]) && char.IsDigit(text[end - tag.Close.Length]);
        }
        
        protected bool HasThisToken(int position, string text)
        {
            if (position + tag.Open.Length >= text.Length)
                return false;
            return text.Substring(position, tag.Open.Length) == tag.Open;
        }
    }
}