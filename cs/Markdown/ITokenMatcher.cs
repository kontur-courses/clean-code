using System.Web.UI;

namespace Markdown
{
    public interface ITokenMatcher
    {
        string TargetString { set; }
        HtmlTextWriterTag Tag { get; }
        
        bool TryOpen(int matchStartIndex, out Range openTagRange);
        bool TryClose(int matchStartIndex, out Range closeTagRange);
    }
}