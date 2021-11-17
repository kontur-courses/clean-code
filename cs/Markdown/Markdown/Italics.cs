namespace Markdown
{
    public class Italics : ISelectionSymbol
    {
        public bool IsClosed { get; set; }


        public string SimpleChar => "_";

        public bool IsStartTag { get; set; }

        public bool IsPossibleStartElement { get; set; }

        public bool IsPossibleEndElement { get; set; }

        public string HtmlTagAnalog
        {
            get
            {
                if (IsClosed)
                    return (IsStartTag) ? "<em>" : "</em>";
                return SimpleChar;
            }
        }
    }
}
