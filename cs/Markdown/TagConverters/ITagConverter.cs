using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal interface ITagConverter
    {
        public abstract StringOfset Convert(string text, int position);
    }
}
