using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        public static string ParseToHtml(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private string ParseTag(Tag tag)
        {
            switch (tag)
            {
                case Tag.Em:
                    return "em";
                case Tag.Strong:
                    return "strong";
                case Tag.Raw:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(tag), tag, null);
            }
        }
    }
}
