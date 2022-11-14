using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenParser
    {
        public static List<Token> ParseLine(string line)
        {
            var tokensList = new List<Token>();
            var index = 0;
            while (index < line.Length)
            {
                tokensList.Add(AddTokens(index, line));
                index = AddTokens(index, line).GetIndexNextToToken();
            }

            return tokensList;
        }

        public static Token AddTokens(int index, string line)
        {
            TokenType type = TokenType.defaultToken;
            return ReadQuotedField(line, index, type);
        }


        public static Token ReadQuotedField(string line, int startIndex, TokenType type)
        {
            var trueLine = new StringBuilder();
            var index = startIndex;
            if (type != TokenType.defaultToken)
                index++;
            var lineLength = line.Length;
            while (index != line.Length)
            {
                
                if (line[index] == '_')
                {
                     
                }

                if (line[index] == '\\')
                {
                    trueLine.Append(line[index + 1]);
                    index++;
                }
                else 
                    trueLine.Append(line[index]);
                index++;
            }

            return new Token(trueLine.ToString(), startIndex, lineLength, type);
        }
    }
}
