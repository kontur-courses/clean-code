namespace Markdown
{
    public class Bold : ISelectionSymbol
    {
        public bool IsClosed { get; set; }

        public string HtmlTagAnalog
        {
            get
            {
                if (IsClosed)
                    return (IsStartTag) ? "<strong>" : "</strong>";
                return SimpleChar;
            }
        }
        public bool IsPossibleStartElement { get; set; }

        public bool IsPossibleEndElement { get; set; }

        public bool IsStartTag { get; set; }

        public string SimpleChar => "__";
    }
}
