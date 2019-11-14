using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    interface ITranslator
    {
        string Render(string inputDocument);
    }
}
