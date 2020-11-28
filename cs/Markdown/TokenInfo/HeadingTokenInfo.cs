using System;

namespace Markdown.TokenInfo
{
    public  class HeadingTokenInfo : ITokenInfo
    {
        public TagType TagType { get; } = TagType.Heading;

        public TagType[] NestedTypes { get; } = {
            TagType.Bold,
            TagType.Italics,
            TagType.Text
        };

        public bool TryReadToken(int startIndex, int finishIndex, string text, out Token token)
        {
            if (text is null)
                throw new ArgumentNullException("Текст был null");

            if (startIndex < 0 || finishIndex >= text.Length || startIndex > finishIndex)
                throw new ArgumentException("Индексы для данного текста некоректны.");

            token = default;
            if (!IsHeadingTag(startIndex, text))
                return false;
            var currentIndexTag = startIndex;
            while (text[currentIndexTag] != '\n' || text[currentIndexTag] != '\r')
            {
                currentIndexTag += 1;
                if (currentIndexTag >= finishIndex)
                    break;
                
            }

            token = new Token(TagType.Heading, startIndex, currentIndexTag);
            return true;
        }
        
        public int GetValueStartIndex(int tagStartIndex)
        {
            return tagStartIndex + 2;
        }

        public int GetValueFinishIndex(int tagFinishIndex)
        {
            return tagFinishIndex;
        }

        public TagType GetType()
        {
            throw new NotImplementedException();
        }

        private bool IsHeadingTag(int index, string text)
        {
            return text[index] == '#'
                   && index + 1 != text.Length
                   && text[index + 1] == ' '
                   && (index == 0 || index - 1 < 0 || text[index - 1] == '\n' || text[index - 1] == '\r');
        }
    }
}
