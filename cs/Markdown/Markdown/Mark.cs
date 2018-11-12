using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class Mark
    {
        public static readonly Mark RawMark = new Mark("", "", "");

        public readonly string Sign;

        public readonly string OpeningTag;

        public readonly string ClosingTag;

        public Mark(string sign, string openingTag, string closingTag)
        {
            Sign = sign;
            OpeningTag = openingTag;
            ClosingTag = closingTag;
        }

        public int FindOpeningIndex(string text,int startIndex)
        {
            while (startIndex < text.Length - Sign.Length * 2)
            {
                if (text.Substring(startIndex, Sign.Length) == Sign && 
                    !Char.IsWhiteSpace(text[startIndex + Sign.Length]) && 
                    !IsBetweenDigits(text, startIndex) &&
                    AfterIsNotTheSameMark(text, startIndex)&& 
                    !IsBackSlashed(text, startIndex))
                    return startIndex;
                startIndex++;
            }

            return -1;
        }

        public int FindClosingIndex(string text,int openingIndex)
        {
            var closingIndex = openingIndex + Sign.Length;
            while (closingIndex <= text.Length - Sign.Length)
            {
                if (text.Substring(closingIndex, Sign.Length) == Sign &&
                    closingIndex > 0 &&
                    !Char.IsWhiteSpace(text[closingIndex - 1]) && 
                    !IsBetweenDigits(text,closingIndex) &&
                    BeforeIsNotTheSameMark(text, closingIndex) &&
                    !IsBackSlashed(text, closingIndex))
                    return closingIndex;
                closingIndex++;
            }

            return -1;
        }

        private bool IsBetweenDigits(string text, int index)
        {
            return BeforeIsDigit(text, index) && AfterIsDigit(text, index);
        }

        private bool BeforeIsDigit(string text, int index)
        {
            while (index > 0)
            {
                if (Char.IsDigit(text[index - 1]))
                    return true;
                if (index - Sign.Length > -1 && text.Substring(index - Sign.Length, Sign.Length) == Sign)
                    index -= Sign.Length;
                else
                    return false;
            }

            return false;
        }

        private bool AfterIsDigit(string text, int index)
        {
            while (index + Sign.Length < text.Length)
            {
                if (Char.IsDigit(text[index + Sign.Length]))
                    return true;
                if (index + 2 * Sign.Length < text.Length && text.Substring(index + Sign.Length, Sign.Length) == Sign)
                    index += Sign.Length;
                else
                    return false;
            }

            return false;
        }

        private bool AfterIsNotTheSameMark(string text, int index)
        {
            return !(index + Sign.Length < text.Length && 
                     text.Substring(index + 1, Sign.Length) == Sign);
        }

        private bool BeforeIsNotTheSameMark(string text, int index)
        {
            return !(index - Sign.Length >-1 && 
                     text.Substring(index - Sign.Length, Sign.Length) == Sign && !IsBackSlashed(text, index-Sign.Length));
        }

        private bool IsBackSlashed(string text, int index)
        {
            var slashCount = 0;
            while (index - slashCount > 0 && text[index-slashCount-1 ]=='\\')
                slashCount++;
            return slashCount % 2 == 1;
        }
    }
}
