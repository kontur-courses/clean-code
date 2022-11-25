using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public interface ITokenTyper<TType> where TType : Enum
    {
        public TType GetSymbolType(int i);
    }
}
