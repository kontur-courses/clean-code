using System;

namespace Markdown.TokenInfo
{
    public class ItalicsTokenInfo : ITokenInfo
    {
        public TagType TagType { get; } = TagType.Italics;

        public TagType[] NestedTypes { get; } =
        {
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
            return tagStartIndex + 1;
        }

        public int GetValueFinishIndex(int tagFinishIndex)
        {
            return tagFinishIndex - 1;
        }

        public TagType GetType()
        {
            throw new NotImplementedException();
        }

        private bool IsItalicsTag(int index, string text)
        {
            throw new NotImplementedException();
        }
    }
}