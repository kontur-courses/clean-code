using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Specification
    {
        public static Dictionary<TokenType, List<TokenType>> possibleNesting =
            new Dictionary<TokenType, List<TokenType>>
            {
                {TokenType.italic, new List<TokenType>() {TokenType.italic}},
                {TokenType.bold, new List<TokenType>() {TokenType.bold, TokenType.italic}}
            };


    }
}