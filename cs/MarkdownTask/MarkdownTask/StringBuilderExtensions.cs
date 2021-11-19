using System.Text;

namespace MarkdownTask
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder WrapTo(this StringBuilder sb, string leftPart, string rightPart)
        {
            return sb.Insert(0, leftPart).Append(rightPart);
        }
    }
}