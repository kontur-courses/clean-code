using Markdown;
using System;


namespace MarkdownUnitTests.Extensions
{
    public static class TokenExtensions
    {
        public static bool IsTag(this Token token)
        {
            if (token.Content != null && token.Tag == null)
                return false;
            return true;
        }
    }
}
