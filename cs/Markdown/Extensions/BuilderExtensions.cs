using System.Text;

namespace Markdown.Extensions;

public static class BuilderExtensions
{
    public static void AppendEscapedCharacter(this StringBuilder builder, string text, int index)
    {
        builder.Append(index + 1 == text.Length ? '\\' : text[index + 1]);
    }
}