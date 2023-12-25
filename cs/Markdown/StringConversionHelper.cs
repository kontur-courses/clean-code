using System.Text;

namespace Markdown;

public class StringConversionHelper
{
    public bool EscapingChar = false;
    public bool H1 = false;
    public readonly bool[] OperationalCharacters;
    public readonly StringBuilder Sb;

    public StringConversionHelper(string text)
    {
        Sb = new StringBuilder();
        OperationalCharacters = new bool[text.Length + 1];
    }

}