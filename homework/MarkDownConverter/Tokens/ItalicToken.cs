using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public class ItalicToken(int position) : Token(position)
    {
        public override TokenState State => TokenState.Italic;

        public override string Value => "_";
    }
}
