using System;

namespace Markdown
{
    public interface ISpanElement
    {
        String GetOpeningIndicator();
        String GetClosingIndicator();
        String ToHtml(String markdown);
        bool Contains(ISpanElement spanElement);
    }
}
