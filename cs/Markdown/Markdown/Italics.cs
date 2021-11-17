using System;

namespace Markdown
{
    public class Italics : ISelectionSymbol
    {
        public bool IsClosed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public string SimpleChar => "_";

        public bool IsStartTag { get; set; }

        public string HtmlTagAnalog { get => (IsStartTag) ? "<em>" : "</em>"; }
    }
}
