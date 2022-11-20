using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers;

namespace Markdown.Renderers
{
    public interface IRenderer
    {
        string Render(ParsedDocument parsedDocument);
    }
}
