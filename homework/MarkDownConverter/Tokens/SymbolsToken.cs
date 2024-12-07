using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public class SymbolsToken(int position, string value) : Token(position)
    {
        public override TokenState State => TokenState.Symbols;

        public override string Value => value;
    }
}
