
namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var result = "";
            StringOfset stringOfset;
            for(var i = 0; i < text.Length; i += stringOfset.ofset)
            {
                stringOfset = TagsAssociation.GetTagConverter(text, i).Convert(text, i);
                result += stringOfset.text;
            }
            return result;
        }
    }
}
