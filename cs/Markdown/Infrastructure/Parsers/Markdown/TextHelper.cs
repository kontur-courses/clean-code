namespace Markdown.Infrastructure.Parsers.Markdown
{
    public class TextHelper
    {
        public readonly string Text;

        public TextHelper(string text)
        {
            Text = text;
        }

        public bool CharIs(char possibleChar, int offset) => 
            IsInBounds(offset) && Text[offset] == possibleChar;

        public bool CharIsNumber(int offset)
        {
            for (var i = 0; i < 10; i++)
                if (CharIs(i.ToString()[0], offset))
                    return true;

            return false;
        }

        public bool CharIsTextChar(int offset) =>
            !CharIsWhiteSpace(offset)
            && !CharIsNumber(offset);

        public bool CharIsWhiteSpace(int offset) =>
            CharIs(' ', offset)
            || CharIs('\t', offset);

        public bool IsInBounds(int offset) => 
            offset >= 0 && offset < Text.Length;

        private bool CharBetweenTags(char c, TagInfo start, TagInfo end)
        {
            for (var offset = start.Offset + start.Length; offset < end.Offset; offset++)
                if (CharIs(c, offset))
                    return true;

            return false;
        }

        public bool WhiteSpaceCharBetweenTags(TagInfo start, TagInfo end) =>
            CharBetweenTags(' ', start, end)
            || CharBetweenTags('\t', start, end);
    }
}