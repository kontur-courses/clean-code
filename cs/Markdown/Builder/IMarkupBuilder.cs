using System.Collections.Generic;

namespace Markdown.Builder
{
    public interface IMarkupBuilder
    {
        public string Build(TextData textData);
    }
}