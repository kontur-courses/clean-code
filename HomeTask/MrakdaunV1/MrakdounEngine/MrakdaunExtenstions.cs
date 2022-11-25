using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using MrakdaunV1.Enums;

namespace MrakdaunV1
{
    public static class MrakdaunExtenstions
    {
        private static readonly char[] _delimiters = /*" ,.!?;:"*/" \n\r".ToCharArray();
        private static readonly char[] _spaceChars = /*" ,.!?;:"*/" \n\r\\".ToCharArray();

        // https://stackoverflow.com/a/28336942
        public static IEnumerable<string> SplitWithDelimeter(this string s, params char[] delimiters)
        {
            var parts = new List<string>();
            if (string.IsNullOrEmpty(s)) return parts;
            var iFirst = 0;
            do
            {
                var iLast = s.IndexOfAny(delimiters, iFirst);
                if (iLast >= 0)
                {
                    if (iLast > iFirst)
                        parts.Add(s.Substring(iFirst, iLast - iFirst)); //part before the delimiter
                    parts.Add(new string(s[iLast], 1));//the delimiter
                    iFirst = iLast + 1;
                    continue;
                }

                //No delimiters were found, but at least one character remains. Add the rest and stop.
                parts.Add(s.Substring(iFirst, s.Length - iFirst));
                break;

            } while (iFirst < s.Length);

            return parts;
        }

        public static bool IsDelimeter(this char c)
        {
            return _delimiters.Contains(c);
        }

        public static bool IsSpaceChar(this char c)
        {
            return _spaceChars.Contains(c);
        }

        public static bool ContainsDelimeters(this string s, int softStart, int softEnd)
        {
            return _delimiters.Any(s.Skip(softStart).Take(softEnd - softStart).Contains);
        }

        public static bool IsPossiblePairWith(this TokenPart current, TokenPart prev, bool sliceHasDelimeters)
        {
            var currnetTokPosType = current.PositionType;
            var prevPosType = prev.PositionType;

            // 4 проверки - 4 условия существования валидной пары тегов
            // 1 - тег до и после слов, хоть убейся, но будет валиден всегда
            // 2, 3 и 4 случаи так или иначе задействуют часть в слове, а тут нужно учесть,
            // что это не должно быть число, а также слово должно быть одно, без разделителей (пробел)
            return (currnetTokPosType == TokenPartPositionType.AfterWord
                    && prevPosType == TokenPartPositionType.BeforeWord)
                   || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                       && prevPosType == TokenPartPositionType.BeforeWord
                       && !sliceHasDelimeters
                       && !current.IsInsideTheNumber)
                   || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                       && prevPosType == TokenPartPositionType.InsideTheWord
                       && !sliceHasDelimeters
                       && !current.IsInsideTheNumber
                       && !prev.IsInsideTheNumber)
                   || (currnetTokPosType == TokenPartPositionType.AfterWord
                       && prevPosType == TokenPartPositionType.InsideTheWord
                       && !sliceHasDelimeters
                       && !prev.IsInsideTheNumber);
        }
    }
}