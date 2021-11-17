using System;

namespace Markdown
{
    public class Bold : ISelectionSymbol
    {
        public bool IsClosed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string HtmlTagAnalog { get => (IsStartTag) ? "<strong>" : "</strong>"; }

        public bool IsStartTag { get; set; }

        public string SimpleChar => "__";
    }
}
