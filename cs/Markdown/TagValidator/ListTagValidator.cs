using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.TagValidator
{
    public class ListTagValidator : ITagValidator
    {
        public bool IsValid(string text, ITag tag, SubTagOrder order, int start)
        {
            return true;
        }
    }
}
