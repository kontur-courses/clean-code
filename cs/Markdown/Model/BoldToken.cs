using Markdown.Interfaces;

namespace Markdown.Model
{
    public class BoldToken : IToken
    {
        public static string MarkTag { get; private set; } = "__";

        public bool IsOpenTag { get; set; }

        public string ConvertToHtml()
        {
            return IsOpenTag ? "<strong>" : "</strong>";
        }
    }
}