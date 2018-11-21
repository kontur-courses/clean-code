using System;
using System.Threading;

namespace Markdown
{
    public static class TokenReader
    {
        public static bool IsNumberWithUnderlines(string input, int begin, int end)
        {
            var currentPos = begin;
            var hasNumbers = false;
            
            while (currentPos < end)
            {
                var isNumber = IsNumber(input, currentPos);
                var isUnderline = IsUnderline(input, currentPos);

                if (!hasNumbers && isNumber)
                {
                    hasNumbers = true;
                }
                
                if (!(isNumber || isUnderline))
                {
                    return false;
                }
                
                currentPos++;
            }

            return hasNumbers;
        }
        
        public static bool TryParseText(string input, int begin, int end, out Token token)
        {
            token = new Token()
            {
                Position = begin,
                Length =  end - begin,
            };
            
            return token.Length > 0 && begin >= 0 && end <= input.Length;
        }
        
        public static bool TryParseSingleUnderlineTag(string input, int begin, int end, out Token token)
        {
            return TryParseTag(input, begin, end, "_", out token);
        }
        
        public static bool TryParseDoubleUnderlineTag(string input, int begin, int end, out Token token)
        {
            return TryParseTag(input, begin, end, "__", out token);
        }
        
        private static bool TryParseTag(string input, int begin, int end, string tagChars, out Token token)
        {
            if (!IsOpenTag(input, begin, end, tagChars))
            {
                token = new Token();
                return false;
            }

            token = new Token();
            var closeTagPos = begin + tagChars.Length;
            
            while (!IsCloseTag(input, closeTagPos++, end, tagChars))
            {
                if (closeTagPos >= end)
                {
                    return false;
                }
                
                var totalLength = closeTagPos + tagChars.Length - begin;
                
                token = new Token()
                {
                    Position = begin,
                    Length = totalLength,
                    ContentLeftOffset = tagChars.Length,
                    ContentLength = totalLength - tagChars.Length * 2
                };
            }

            return token.ContentLength > 0;
        }
        
        private static bool IsOpenTag(string input, int begin, int end, string tagChars)
        {
            if (begin + tagChars.Length > end)
            {
                return false;
            }
            
            var containsTagChars = input
                .Substring(begin, tagChars.Length)
                .Equals(tagChars);
            
            var isBeginOfInput = begin == 0;
            var isEndOfInput = (begin + tagChars.Length) >= input.Length;
            var startsWithSpace = !isBeginOfInput && IsWhiteSpace(input, begin - 1);
            var endsWithLetter = !isEndOfInput && IsLetterOrDigit(input, begin + tagChars.Length);
            return containsTagChars && (isEndOfInput || endsWithLetter) && (isBeginOfInput || startsWithSpace);
        }

        private static bool IsCloseTag(string input, int begin, int end, string tagChars)
        {
            if (begin + tagChars.Length > end)
            {
                return false;
            }
            
            var containsTagChars = input
                .Substring(begin, tagChars.Length)
                .Equals(tagChars);

            var isBeginOfInput = begin == 0;
            var isEndOfInput = (begin + tagChars.Length) >= input.Length;
            var startsWithLetter = !isBeginOfInput && IsLetterOrDigit(input, begin - 1);
            var endsWithSpace = !isEndOfInput && IsWhiteSpace(input, begin + tagChars.Length);
            return containsTagChars && (isBeginOfInput || startsWithLetter) && (isEndOfInput || endsWithSpace);
        }
        
        private static bool IsWhiteSpace(string input, int pos)
        {
            return char.IsWhiteSpace(input[pos]);
        }
        
        private static bool IsLetterOrDigit(string input, int pos)
        {
            return char.IsLetterOrDigit(input[pos]);
        }
        
        private static bool IsNumber(string input, int pos)
        {
            return char.IsDigit(input[pos]);
        }
        
        private static bool IsUnderline(string input, int pos)
        {
            return input[pos].Equals('_');
        }
    }
}