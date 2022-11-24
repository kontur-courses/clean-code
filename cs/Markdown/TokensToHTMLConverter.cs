using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokensToHTMLConverter
    {
        /*Формирование html строки по последнему списку токенов*/
        public string Convert(List<SecondLevelToken> semanticTokenList)
        {
            var sb = new StringBuilder();
            var isHeader = false;
            var length = semanticTokenList.Count;
            for (int i = 0; i < length; i++)
            {
                var tokenType = semanticTokenList[i].GetSecondTokenType();
                var tokenValue = semanticTokenList[i].GetTokenValue();
                if (CheckIsItHeader(semanticTokenList, i, tokenType, length))
                {
                    isHeader = true;
                    sb.Append("\\<h1>");
                    i += 1;
                    continue;
                }

                switch (tokenType)
                {
                    case SecondLevelTokenType.OpeningItalics:
                        sb.Append("\\<em>");
                        continue;
                    case SecondLevelTokenType.ClosingItalics:
                        sb.Append("\\</em>");
                        continue;
                    case SecondLevelTokenType.OpeningBold:
                        sb.Append("\\<strong>");
                        continue;
                    case SecondLevelTokenType.ClosingBold:
                        sb.Append("\\</strong>");
                        continue;
                    case SecondLevelTokenType.Backslash:
                        continue;
                    default:
                        sb.Append(tokenValue);
                        break;
                }
            }

            if (isHeader)
                sb.Append("\\</h1>");
            return sb.ToString();
        }

        private static bool CheckIsItHeader(List<SecondLevelToken> semanticTokenList, int i, SecondLevelTokenType tokenType, int length)
        {
            return i == 0 && tokenType == SecondLevelTokenType.Header &&
                   length > 2 && 
                   semanticTokenList[1].GetSecondTokenType() == SecondLevelTokenType.Space;
        }
    }
}