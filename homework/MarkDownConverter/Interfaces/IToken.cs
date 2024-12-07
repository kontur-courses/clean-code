using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Interfaces
{
    public interface IToken
    {
        public TokenState State { get; }
        public int Position { get; }
        public int Length { get; }
        public string Value { get; }
        bool Is(TokenState state);
    }
}
