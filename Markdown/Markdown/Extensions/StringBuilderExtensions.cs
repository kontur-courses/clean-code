using System.Text;

namespace Markdown.Extensions;

public static class StringBuilderExtensions
{
    public static void AppendHtml(this StringBuilder sb, string htmlTag, string innerContent)
    {
        sb.Append($"<{htmlTag}>{innerContent}</{htmlTag}>");
    }
}