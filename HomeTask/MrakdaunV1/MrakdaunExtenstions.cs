using System.Linq;

namespace MrakdaunV1
{
    public static class MrakdaunExtenstions
    {
        private static char[] _delimeters = " ,.!?;:".ToCharArray();
        
        public static bool IsDelimeter(this char c)
        {
            return _delimeters.Contains(c);
        }

        public static bool ContainsDelimeters(this string s, int softStart, int softEnd)
        {
            return _delimeters.Any(s.Skip(softStart).Take(softEnd - softStart).Contains);
        }
        
        public static bool ContainsSingleCharsWithoutRepeat(this string s, int softStart, int softEnd, char pattern)
        {
            var slice = s.Skip(softStart).Take(softEnd - softStart).ToString();

            for (int i = 0; i < slice.Count() - 1; i++)
            {
                // Обнаружены повторения символа
                if (slice[i] == pattern && slice[i + 1] == pattern)
                    return false;
            }

            return true;
        }
        
        /*public static bool ContainsFixedLengthCharsWithoutRepeat(this string s, int softStart, int softEnd, char pattern, int fixedLength)
        {
            var slice = s.Skip(softStart).Take(softEnd - softStart).ToString();

            for (int i = 0; i < slice.Count() - fixedLength; i++)
            {
                if (slice[i] == pattern && slice.Skip(i).Take(fixedLength-1).All(c => c == pattern))
                {
                    if (slice[i+fixedLength] == pattern && slice.Skip(i+fixedLength).Take(2*fixedLength-1).All(c => c == pattern))
                    {
                    
                    }
                }
            }

            return true;
        }*/
    }
}