using System;

namespace MarkDown
{
    public static class StringExtensions
    {
        public static char Last(this string line)
        {
            if(line.Length == 0)
                throw new IndexOutOfRangeException();
            return line[line.Length - 1];
        }
    }
}