using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Complex
    {
        public static readonly HashSet<TokenProperty> Properties = [
            TokenProperty.Head,
            TokenProperty.Paragraph,
            TokenProperty.Bold
        ];
    }
}
