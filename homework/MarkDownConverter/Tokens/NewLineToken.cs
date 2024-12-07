using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public class NewLineToken(int position) : Token(position)
    {
        public override TokenState State => TokenState.NewLine;

        public override string Value => "\n";
    }
}
