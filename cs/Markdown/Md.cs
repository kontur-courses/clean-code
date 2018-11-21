using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var tagReader = new TagReader(text, new StrongTagInfo(), new EmTagInfo());
            return tagReader.Evaluate();
        }
    }
}
