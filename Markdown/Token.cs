using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token(string value, TokenProperty property)
    {
        public string Value { get; set; } = value;
        public readonly TokenProperty Property = property;
        public List<Token> Children = [];

        public override string ToString()
        {
            return $"{Value}       {Property}";
        }
    }
}
