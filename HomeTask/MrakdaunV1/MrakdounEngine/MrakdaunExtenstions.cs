using System.Linq;

namespace MrakdaunV1
{
    public static class MrakdaunExtenstions
    {
        private static char[] _delimeters = /*" ,.!?;:"*/" ".ToCharArray();
        
        public static bool IsDelimeter(this char c)
        {
            return _delimeters.Contains(c);
        }

        public static bool ContainsDelimeters(this string s, int softStart, int softEnd)
        {
            return _delimeters.Any(s.Skip(softStart).Take(softEnd - softStart).Contains);
        }
    }
}