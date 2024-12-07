using MarkDownConverter.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Interfaces
{
    public interface IParser
    {
        public ParentNode Parse(List<IToken> tokens);
    }
}
