using System.Collections.Generic;
using Markdown.Tag_Classes;

namespace Markdown
{
    public interface ITaginizer
    {
        List<TagEvent> GetTagEvents();
    }
}
