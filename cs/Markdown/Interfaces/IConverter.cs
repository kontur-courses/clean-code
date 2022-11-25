using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tokens;

namespace Markdown.Interfaces
{
    public interface IConverter
    {
        public string ConvertTokens(List<Token> tokens);
    }
}
