using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization
{
    public static class TokenizationContextExtensions
    {
        public static string GetFollowingText(this TokenizationContext context, int length)
        {
            return length + context.CurrentStartIndex >= context.Source.Length
                ? context.Source.Substring(context.CurrentStartIndex)
                : context.Source.Substring(context.CurrentStartIndex, length);
        }
    }
}