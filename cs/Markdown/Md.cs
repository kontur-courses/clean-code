using System.Text;

namespace Markdown;

public class Md
{
    private readonly Stack<TextSegment> _segmentsStack = new Stack<TextSegment>();
    public string Render(string text)
    {
        throw new NotImplementedException();
    }

    private bool IsEscapeSymbolBeforeMarkdownSymbol(string text, int indexOfEscapeSymbol)
    {
        throw new NotImplementedException();
    }
}