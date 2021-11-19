namespace Markdown
{
    public class TagItalic : ITag
    {
        public bool IsAtTheBeginning { get; set; }
        public bool IsClosed { get; set; }

        public string HtmlTagAnalog
        {
            get
            {
                if (IsClosed)
                    return (IsStartTag) ? "<em>" : "</em>";
                return "_";
            }
        }

        public bool IsStartTag { get; set; }

        public string Content => HtmlTagAnalog;

        public bool IsPrevent { get; set; }
    }
}
