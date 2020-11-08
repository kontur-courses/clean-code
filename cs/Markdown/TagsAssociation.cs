using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal static class TagsAssociation
    {
        internal static ITagConverter GetTagConverter(string text, int position)
        {
            return new EmptyTagConverter();
        }
    }
}
