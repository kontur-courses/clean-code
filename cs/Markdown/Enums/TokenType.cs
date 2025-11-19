using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Enums
{
    /// <summary>
    /// <para>HeaderMarker = #</para>
    /// <para>BoldMarker = __</para>
    /// <para>ItalicsMarker = _</para>
    /// </summary>
    public enum TokenType
    {
        Text,
        Newline,
        HeaderMarker,
        BoldMarker,
        ItalicMarker
    }
}
