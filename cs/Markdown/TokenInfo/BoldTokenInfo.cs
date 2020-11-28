using System;

namespace Markdown.TokenInfo
{
    public class BoldTokenInfo : ITokenInfo
    {
        public TagType TagType { get; } = TagType.Bold;

        public TagType[] NestedTypes { get; } = {
            TagType.Italics,
            TagType.Text
        };

        public bool TryReadToken(int startIndex, int finishIndex, string text, out Token token)
        {
            if (text is null)
                throw new ArgumentNullException("Текст был null");

            if (startIndex < 0 || finishIndex >= text.Length)
                throw new ArgumentException("Индексы для данного текста некоректны.");

            token = default;
            return false;
        }

        public int GetValueStartIndex(int tagStartIndex)
        {
            return tagStartIndex + 2;
        }

        public int GetValueFinishIndex(int tagFinishIndex)
        {
            return tagFinishIndex - 2;
        }

        public TagType GetType()
        {
            return TagType;
        }

        private bool IsBoldType(int index, string text)
        {
            throw new NotImplementedException();
        }
    }
}
