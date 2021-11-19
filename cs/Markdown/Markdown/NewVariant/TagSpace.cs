using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TagSpace : IToken
    {
        public string Content => " ";

        public bool IsPrevent
        {
            get => false;

            set => throw new NotImplementedException();
        }
    }
}
