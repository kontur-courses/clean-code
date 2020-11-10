using System;

namespace Markdown
{
    public class Tag
    {
        public string TagName { get; }
        public int OpenPosition { get; }
        public int ClosePosition { get; }
        
        public static Tag SpotTag(string text, int index)
        {
            throw new NotImplementedException();
        }

        public int FindClosePosition(string text)
        {
            throw new NotImplementedException();
        }
    }
}