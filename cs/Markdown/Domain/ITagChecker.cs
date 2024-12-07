using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Domain.Tags;

namespace Markdown.Domain
{
    public interface ITagChecker
    {
        Tuple<Tag, int> GetTagType(string line, int index);
    }
}
