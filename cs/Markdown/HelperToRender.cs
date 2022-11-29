using System.Text;

namespace Markdown;

public class HelperToRender
{
    public bool escapeSymbol = false;
    public bool h1 = false;
    public bool[] operationalCharacters;
    public StringBuilder sb;

    public HelperToRender(string text)
    {
        sb = new StringBuilder();
        operationalCharacters = new bool[text.Length + 1];
    }
}