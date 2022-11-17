using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tags;

namespace Markdown.Renders
{
    public interface IRender
    {
        string Render(IEnumerable<ITag> tags);
    }
}
