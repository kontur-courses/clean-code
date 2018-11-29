namespace MarkdownNew
{
    class Tag : ITag
    {
        public readonly string Open;
        public readonly string Close;

        public Tag(string open, string close)
        {
            Close = close;
            Open = open;
        }

        private bool IsDigitNextToTag(string line, int tagStart, int tagEnd)
        {
            return tagStart - 1 >= 0 &&
                    char.IsDigit(line[tagStart - 1]) &&
                    tagEnd + 1 < line.Length &&
                    char.IsDigit(line[tagEnd + 1]);
        }

        private bool IsSpaceBeforeCloseTag(string line, int tagStart)
        {
            return tagStart - 1 < 0 || line[tagStart - 1] == ' ';
        }

        private bool IsSpaceAfterOpenTag(string line, int tagEnd)
        { 
            return tagEnd + 1 >=line.Length || line[tagEnd + 1] == ' ';
        }

        private bool IsUnderlineNearTag(string convertingString, int tagStart, int tagEnd)
        {
            return tagStart - 1 >= 0 &&
                   convertingString[tagStart - 1] == '_' ||
                   tagEnd + 1 < convertingString.Length &&
                   convertingString[tagEnd + 1] == '_';
        }

        public bool IsValidOpenTagFromPosition(string convertingString, int position)
        {
            var tagEnd = position + Open.Length - 1;
            if (!IsTagFromPosition(convertingString, position, Open)) return false;
            if (IsDigitNextToTag(convertingString, position, tagEnd)) return false;
            if (IsUnderlineNearTag(convertingString, position, tagEnd)) return false;
            return !IsSpaceAfterOpenTag(convertingString, position + Open.Length - 1);
        }

        private bool IsTagFromPosition(string someString, int position, string tag)
        {
            if (position + tag.Length > someString.Length) return false;
            var subStr = "";
            for (var index = 0; index < tag.Length; index++)
                subStr += someString[position + index];
            return subStr == tag;
        }

        public bool IsValidCloseTagFromPosition(string convertingString, int position)
        {
            var tagEnd = position + Close.Length - 1;
            if (!IsTagFromPosition(convertingString, position, Close)) return false;
            if (IsDigitNextToTag(convertingString, position, tagEnd)) return false;
            if (IsUnderlineNearTag(convertingString, position, tagEnd)) return false;
            return !IsSpaceBeforeCloseTag(convertingString, position);
        }
    
    }
}
