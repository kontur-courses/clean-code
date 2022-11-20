using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tokens
{
    public interface IToken
    {
        int Length { get; }
        int Position { get; }
    }
}
