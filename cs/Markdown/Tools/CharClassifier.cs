using System.Collections.Generic;
using Markdown.Parser.TagsParsing;

namespace Markdown.Tools
{
    public class CharClassifier
    {
        private readonly HashSet<char> tagChars;

        public CharClassifier(IEnumerable<char> tagChars)
        {
            this.tagChars = new HashSet<char>(tagChars);
        }

        public CharType GetType(char chr)
        {
            if (char.IsWhiteSpace(chr))
                return CharType.SpaceSymbol;
            if (chr == '\\')
                return CharType.Escape;
            if (tagChars.Contains(chr))
                return CharType.TagSymbol;

            return CharType.OtherSymbol;
        }
    }
}