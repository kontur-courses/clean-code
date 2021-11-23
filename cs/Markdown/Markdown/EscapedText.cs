using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class EscapedText
    {
        public string Text { get; }

        private readonly List<int> escapedSymbolsBefore;

        public EscapedText(string text, List<int> escapedSymbolsBefore)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            this.escapedSymbolsBefore =
                escapedSymbolsBefore ?? throw new ArgumentNullException(nameof(escapedSymbolsBefore));
        }

        public int GetPositionOffset(int position) =>
            Text.Length > position
                ? escapedSymbolsBefore[position]
                : escapedSymbolsBefore.Last();
    }
}