using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    interface ITag : IToken
    {

        bool IsClosed
        { get; set; }

        string HtmlTagAnalog
        { get; }

        string SimpleChar
        { get; }

        bool IsStartTag
        { get; set; }

        bool IsAtTheBeginning
        { get; set; }

        
    }
}
