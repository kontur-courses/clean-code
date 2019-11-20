using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities.TokenProcessingDescriptions
{
    public abstract class PairedTokenProcessingDescription : TokenProcessingDescription
    {
        public abstract bool IsOpening(List<Token> tokens, int tokenIndex);
        public abstract bool IsClosing(List<Token> tokens, int tokenIndex);
        public abstract string GetRenderedTokenText(Token token, bool hasPair, bool isClosed);
    }
}
