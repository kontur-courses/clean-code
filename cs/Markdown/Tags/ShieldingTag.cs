using System.Collections.Generic;

namespace Markdown
{
    public class ShieldingTag : ITag
    {
        public string OpeningMarkup => "\\";
        public string ClosingMarkup => "";
        public string OpeningTag => "";
        public string ClosingTag => "";

        public void Replace(List<string> builder, int start, int end)
        {
            builder[start] = "";
        }

        public bool IsBrokenMarkup(string source, int start, int length)
        {
            return false;
        }
    }
}