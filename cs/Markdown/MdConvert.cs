using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class MdConvert
    {
        private readonly static Dictionary<TokenType, string> tokenPrefix;
        private readonly static Dictionary<TokenType, string> tokenSuffix;

        static MdConvert()
        {
            tokenPrefix = new Dictionary<TokenType, string> { { TokenType.Bold,  "<strong>" },
                                                                { TokenType.Italic, "<em>"} };
            tokenSuffix = new Dictionary<TokenType, string> { { TokenType.Bold, "</stron>" },
                                                                { TokenType.Italic, "</em>" } };
        }

        public static string ToHtml(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
