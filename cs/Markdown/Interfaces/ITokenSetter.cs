using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tokens;

namespace Markdown.Interfacess
{
    public interface ITokenSetter<T>
    where T : Enum
    {
        public void SetToken(List<Token> tokens, T type, ref int index, string line, StringBuilder builder);
        public void CloseTags(List<Token> tokens);
        public void DeleteEmptyTags(List<Token> tokens);
    }
}
