
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var result = new StringBuilder();
            StringOfset stringOfset;
            for(var i = 0; i < text.Length; i += stringOfset.ofset)
            {
                stringOfset = TagsAssociation.GetTagConverter(text, i).Convert();
                result.Append(stringOfset.text);
            }
            return result.ToString(); ;
        }
    }
}
