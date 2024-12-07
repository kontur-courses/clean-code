using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Interfaces
{
    public interface ITokenizer
    {
        public List<IToken> Tokenize(string text);
    }
}
