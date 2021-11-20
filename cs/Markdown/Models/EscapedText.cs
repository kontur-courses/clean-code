using System.Collections.Generic;
using System.Linq;

namespace Markdown.Models
{
    public class EscapedText
    {
        public string Text { get; }
        public IReadOnlyList<int> EscapedSymbolsBefore { get; }

        public EscapedText(string text, IEnumerable<int> escapedSymbolsBefore)
        {
            // TODO Check arguments
            Text = text;
            EscapedSymbolsBefore = escapedSymbolsBefore.ToList();
        }
    }
}