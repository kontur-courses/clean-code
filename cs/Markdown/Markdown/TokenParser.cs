using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Api;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class TokenParser
    {
        public static IEnumerable<Token> ParseByOne(string text, Mark mark)
        {
            var startIndex = 0;
            while (startIndex < text.Length)
            {
                var openingIndex = mark.FindOpeningIndex(text, startIndex);
                var closingIndex = mark.FindClosingIndex(text, openingIndex);

                if (openingIndex == -1)
                {
                    yield return MakeRawTokenToEnd(text, startIndex);
                    break;
                }

                if (openingIndex == -1 || closingIndex == -1)
                {
                    yield return MakeRawTokenToEnd(text, startIndex);
                    break;
                }

                if (openingIndex != startIndex)
                {
                    yield return MakeToken(text, startIndex, openingIndex, Mark.RawMark);
                }

                yield return MakeToken(text, openingIndex, closingIndex, mark);

                startIndex = closingIndex + mark.Sign.Length;
            }
        }

        private static Token MakeRawTokenToEnd(string text, int startIndex)
        {
            var notMarkedText = text.Substring(startIndex);
            return new Token(notMarkedText, Mark.RawMark);
        }

        private static Token MakeToken(string text, int openingIndex, int closingIndex, Mark mark)
        {
            var markedText = text.Substring(openingIndex + mark.Sign.Length,
                closingIndex - openingIndex - mark.Sign.Length);
            return new Token(markedText, mark);
        }
    }
}
