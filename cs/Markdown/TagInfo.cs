using System;

namespace Markdown
{
    public class TagInfo
    {
        public readonly int Length;
        public readonly string Md;
        public readonly string Html;

        public TagInfo(string md, int length, string html)
        {
            if(md.Length == 0) throw new ArgumentException();
            Length = length;
            Md = md;
            Html = html;
        }
    }
}