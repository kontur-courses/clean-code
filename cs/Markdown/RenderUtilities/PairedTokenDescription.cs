using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class PairedTokenDescription
    {
        public readonly Token OpenToken;
        public readonly Token CloseToken;

        public PairedTokenDescription(Token openToken, Token closeToken)
        {
            OpenToken = openToken;
            CloseToken = closeToken;
        }
    }
}
