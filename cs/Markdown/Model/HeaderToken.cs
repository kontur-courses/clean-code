using Markdown.Interfaces;

namespace Markdown.Model
{
    public class HeaderToken : IToken
    {
        public static string MarkTag { get; private set; } = "# ";

        public bool IsOpenTag { get; set; }

        public string ConvertToHtml()
        {
            return IsOpenTag ? "<h1>" : "</h1>";
        }
    }
}