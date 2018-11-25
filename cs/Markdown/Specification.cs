using System;
using System.Collections.Generic;
using Markdown.Types;

namespace Markdown
{
    class Specification
    {
        public Specification()
        {
            foreach (var tokenType in tokenTypes)
            {
                MdToTokenTypes[tokenType.Delimiter] = tokenType;
                var closingDelimiter = tokenType.Delimiter;
                if (!tokenType.IsPair)
                    closingDelimiter = "\r\n";
                MdStartingToClosingDelimiters[tokenType.Delimiter] = closingDelimiter;
                MdDelimiters.Add(tokenType.Delimiter);
                MdDelimiters.Add(closingDelimiter);
            }
        }

        private readonly HashSet<IMdToken> tokenTypes = new HashSet<IMdToken>
        {
            new MdItalic(), new MdBold(),
            new MdHeader1(), new MdHeader2(), new MdHeader3(),
            new MdHeader4(), new MdHeader5(), new MdHeader6()
        };


        public Dictionary<string, string> MdStartingToClosingDelimiters = new Dictionary<string, string>();
        public Dictionary<string, IMdToken> MdToTokenTypes = new Dictionary<string, IMdToken>();
        public HashSet<string> MdDelimiters = new HashSet<string>();


        public HashSet<string> KeyWords = new HashSet<string>() {"\\"};

        public HashSet<string> ProhibitionСharacters = new HashSet<string>
            {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

        public Dictionary<Type, HashSet<Type>> ImpossibleNesting =
            new Dictionary<Type, HashSet<Type>>
            {
                {typeof(MdItalic), new HashSet<Type>() {typeof(MdBold)}}
            };

        public bool CanBeStarting(Substring substring, string markdownInput)
        {
            if (substring.Value.StartsWith("#") && substring.Index > 0 && markdownInput[substring.Index - 1] != '\n')
                return false;
            return substring.Index + substring.Length < markdownInput.Length &&
                   markdownInput[substring.Index + substring.Length] != ' ';
        }

        public bool CanBeClosing(Substring substring, string markdownInput)
        {
            if (substring.Value == "\r\n")
                return true;
            return substring.Index != 0 && markdownInput[substring.Index - 1] != ' ';
        }
    }
}