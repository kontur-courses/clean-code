using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    class Tag : ITag
    {
        public readonly string open;
        public readonly string close;

        public Tag(string open, string close)
        {
            this.close = close;
            this.open = open;
        }

        private bool IsDigitNextToTag(string line, int tagStart, int tagEnd)
        {
            return tagStart - 1 >= 0 &&
                    char.IsDigit(line[tagStart - 1]) &&
                    tagEnd + 1 < line.Length &&
                    char.IsDigit(line[tagEnd + 1]);
        }

        private bool IsSlashBeforeTag(string line, int tagStart)
        {
            return tagStart - 1 >= 0 && line[tagStart - 1] == '\\';
        }

        private bool IsSpaceBeforeTag(string line, int tagStart)
        {
            return tagStart - 1 < 0 || line[tagStart - 1] == ' ';
        }

        private bool IsSpaceAfterTag(string line, int tagEnd)
        { 
            return tagEnd + 1 >=line.Length || line[tagEnd + 1] == ' ';
        }

        public bool IsValidOpenTagFromPosition(string someString, int position)
        {
            if (!IsTagFromPosition(someString, position, open)) return false;
            var tagEnd = position + open.Length - 1;
            if (IsDigitNextToTag(someString, position, tagEnd)) return false;
            if (position - 1 >= 0 &&
                someString[position - 1] == '_' ||
                tagEnd + 1 < someString.Length &&
                someString[tagEnd + 1] == '_') return false;
            return !IsSlashBeforeTag(someString, position) &&
                !IsSpaceAfterTag(someString, position + open.Length - 1);
        }

        private bool IsTagFromPosition(string someString, int position, string tag)
        {
            if (position + tag.Length > someString.Length) return false;
            var tmp = "";
            for (var index = 0; index < tag.Length; index++)
                tmp += someString[position + index];
            return tmp == tag;
        }

        public bool IsValidCloseTagFromPosition(string someString, int position)
        {
            if (!IsTagFromPosition(someString, position, close)) return false;
            var tagEnd = position + close.Length - 1;
            if (IsDigitNextToTag(someString, position, tagEnd)) return false;
            if (position - 1 >= 0 &&
                someString[position - 1] == '_' ||
                tagEnd + 1 < someString.Length &&
                someString[tagEnd + 1] == '_') return false;
            return !IsSlashBeforeTag(someString, position) &&
                !IsSpaceBeforeTag(someString, position);
        }
    
    }
}
