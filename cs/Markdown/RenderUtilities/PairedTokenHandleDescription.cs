using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class PairedTokenHandleDescription
    {
        public readonly TokenType TokenType;

        private readonly Func<List<Token>, int, bool> isOpening;
        private readonly Func<List<Token>, int, bool> isClosing;
        //string printToken(Token token, bool hasPair, bool isClosed)
        private readonly Func<Token, bool, bool, string> printToken;

        public PairedTokenHandleDescription(TokenType tokenType, Func<List<Token>, int, bool> isOpening,
            Func<List<Token>, int, bool> isClosing, Func<Token, bool, bool, string> printToken)
        {
            TokenType = tokenType;
            this.isOpening = isOpening;
            this.isClosing = isClosing;
            this.printToken = printToken;
        }

        public bool IsOpening(List<Token> tokens, int tokenIndex)
        {
            return isOpening(tokens, tokenIndex);
        }

        public bool IsClosing(List<Token> tokens, int tokenIndex)
        {
            return isClosing(tokens, tokenIndex);
        }

        public string PrintToken(Token token, bool hasPair, bool isClosed)
        {
            return printToken(token, hasPair, isClosed);
        }
    }
}
