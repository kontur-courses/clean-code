using System;

namespace Markdown.TokenInfo
{
    public class TextTokenInfo : ITokenInfo
    {
        public TagType TagType { get; }

        public TagType[] NestedTypes { get; } = new TagType[0];

        public bool TryReadToken(int startIndex, int finishIndex, string text, out Token token)
        {
            if (text is null)
                throw new ArgumentNullException("Текст был null");

            if (startIndex < 0 || finishIndex >= text.Length)
                throw new ArgumentException("Индексы для данного текста некоректны.");

            token = default;
            var currentIndex = startIndex;
            token = new Token(TagType.Text, startIndex, finishIndex);
            

            return true;
        }

        public int GetValueStartIndex(int tagStartIndex)
        {
            return tagStartIndex;
        }

        public int GetValueFinishIndex(int tagFinishIndex)
        {
            return tagFinishIndex;
        }

        public TagType GetType()
        {
            throw new NotImplementedException();
        }

        private bool IsSpecialSymbol(char symbol)
        {
            return symbol == '#' || symbol == '_' || symbol == '[';
        }
    }
}
