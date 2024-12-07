using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public class HeadingToken(int position) : Token(position)
    {
        public override TokenState State => TokenState.Heading;

        public override string Value => "#";
    }
}
