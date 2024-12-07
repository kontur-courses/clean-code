using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public class NumbersToken(int position, string value) : Token(position)
    {
        public override TokenState State => TokenState.Numbers;

        public override string Value => value;
    }
}
