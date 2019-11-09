using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Markdown.Parsing;

namespace Markdown.Formats
{
    public class MD
    {
        public static string Render(string paragraph)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<FormatToken<MDToken>> FindTokens(string paragraph)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<FormatToken<HTMLToken>> MDTokens2HTMLTokens(IEnumerable<FormatToken<MDToken>> tokens)
        {
            throw new NotImplementedException();
        }
    }
}