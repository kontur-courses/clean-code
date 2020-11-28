using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenInfo
{
    class EntireTextTokenInfo : ITokenInfo
    {
        public TagType[] NestedTypes { get; } =
        {
            TagType.Heading,
            TagType.Bold,
            TagType.Text,
            TagType.Italics
        };
        public TagType TagType { get; }
        public bool TryReadToken(int startIndex, int finishIndex, string text, out Token token)
        {
            if (text is null)
                throw new ArgumentNullException("Текст был null");

            if (startIndex < 0 || finishIndex >= text.Length)
                throw new ArgumentException("Индексы для данного текста некоректны.");

            token = default;
            var result = startIndex == 0 && finishIndex == text.Length - 1;
            if (result)
                token = new Token(TagType.EntireText, startIndex, finishIndex);

            return result;
        }

        public int GetValueStartIndex(int startIndex)
        {
            return startIndex;
        }

        public int GetValueFinishIndex(int tagFinishIndex)
        {
            return tagFinishIndex;
        }

        public TagType GetType()
        {
            throw new NotImplementedException();
        }
    }
}
