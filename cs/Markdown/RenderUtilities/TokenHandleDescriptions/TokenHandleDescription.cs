using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities.TokenHandleDescriptions
{
    public abstract class TokenHandleDescription
    {
        public abstract TokenType TokenType { get; }
    }
}
