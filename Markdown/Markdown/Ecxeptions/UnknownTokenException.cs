using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Ecxeptions
{
    public class UnknownTokenException : Exception
    {
        private readonly Token unknownToken;

        public UnknownTokenException(Token unknownToken)
        {
            this.unknownToken = unknownToken;
        }

        public override string Message => $"Unknown token type {unknownToken.Type}, value - {unknownToken.Value}.";
    }
}
