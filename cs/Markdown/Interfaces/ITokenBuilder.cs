using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Tokens;

namespace Markdown.Interfaces
{
    public interface ITokenBuilder
    {
        public Tag GetTag(int start, int end, TokenType type);
        public Text GetText(int start, string value);
    }
}
