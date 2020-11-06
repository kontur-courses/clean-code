using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class EmptyTagConverter : ITagConverter
    {
        public StringOfset Convert(string text, int position) => new StringOfset(text[position].ToString(), 1);
    }
}
