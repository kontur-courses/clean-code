namespace Markdown.Infrastructure.Parsers
{
    public interface ITextHelper
    {
        public void Initialise(string text);
        public bool CharIs(char possibleChar, int offset);
        public bool CharIsNumber(int offset);
        public bool CharIsTextChar(int offset);
        public bool CharIsWhiteSpace(int offset);
        public bool IsInBounds(int offset);
        public bool CharBetweenTags(char c, TagInfo start, TagInfo end);
        public bool WhiteSpaceCharBetweenTags(TagInfo start, TagInfo end);
        public int GetTextLength();
        public string Substring(int start, int length);
        public char GetCharacter(int offset);
    }
}