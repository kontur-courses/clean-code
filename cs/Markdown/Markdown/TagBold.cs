namespace Markdown
{
    public class TagBold : ITag

    {
        public bool IsAtTheBeginning { get; set; }
        public bool IsClosed { get; set; }

        public string HtmlTagAnalog
        {
            get
            {
                if (IsClosed)
                    return (IsStartTag) ? "<strong>" : "</strong>";
                return "__";
            }
        }

        public bool IsStartTag { get; set; }

        public string Content => HtmlTagAnalog;

        public bool IsPrevent { get; set; }
    }
}
