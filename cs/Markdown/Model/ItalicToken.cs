using Markdown.Interfaces;

namespace Markdown.Model
{
    public class ItalicToken : IToken
    {
        public static string MarkTag { get; private set; } = "_";

        public bool IsOpenTag { get; set; }

        public string ConvertToHtml()
        {
            return IsOpenTag ? "<em>" : "</em>";
        }
    }
}