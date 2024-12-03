using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public interface IConverter
    {
        public string Convert(DOM dom);
    }
}
