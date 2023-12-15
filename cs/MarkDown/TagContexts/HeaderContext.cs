using System.Text;
using MarkDown.Interfaces;

namespace MarkDown.TagContexts;

public class HeaderContext : ITagContext
{
    public int StartIndex { get; }
    
    public HeaderContext(int startIndex)
    {
        StartIndex = startIndex;
    }
    
    public void DeleteUnsupportedInners(MarkDownEnvironment environment, StringBuilder sb)
    {
        return;
    }

    
}