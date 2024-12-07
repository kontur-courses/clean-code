using MarkDownConverter.Enums;
using MarkDownConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Tokens
{
    public abstract class Token(int position) : IToken
    {
        public abstract TokenState State { get; }

        public abstract string Value { get; }

        public int Position => position;

        public int Length => Value.Length;

        public bool Is(TokenState state) => State == state;
    }
}
