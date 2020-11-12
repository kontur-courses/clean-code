
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var texts = text.Split('\n');
            var result = new StringBuilder();
            StringOfset stringOfset;
            for (var t = 0; t < texts.Length; t++)
            {
                for (var i = 0; i < texts[t].Length; i += stringOfset.ofset)
                {
                    stringOfset = TagsAssociation.GetTagConverter(text, i).Convert();
                    result.Append(stringOfset.text);
                }
                if (t != texts.Length - 1)
                    result.Append('\n');
            }
            return result.ToString(); ;
        }
    }
}
