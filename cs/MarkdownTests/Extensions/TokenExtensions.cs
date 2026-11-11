using Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTests.Extensions
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
