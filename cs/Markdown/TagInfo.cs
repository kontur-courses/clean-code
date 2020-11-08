using System.Collections.Generic;

namespace Markdown
{
    public class TagInfo
    {
        public static readonly HashSet<char> MdFirstChars = new HashSet<char>();
        
        public readonly int Length;
        public readonly string Html;

        public TagInfo(string md, int length, string html)
        {
            Length = length;
            Html = html;
            if (md.Length > 0) MdFirstChars.Add(md[0]);
        }
    }
}