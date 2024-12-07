using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public class SpaceToken(int position) : Token(position)
    {
        public override TokenState State => TokenState.Space;

        public override string Value => " ";
    }
}
