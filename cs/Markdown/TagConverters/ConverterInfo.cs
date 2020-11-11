using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class ConverterInfo
    {
        private readonly string text;
        private readonly int position;
        private readonly ITagConverter tagConverter;
        internal ConverterInfo(ITagConverter tagConverter, string text, int position)
        {
            this.tagConverter = tagConverter;
            this.text = text;
            this.position = position;
        }

        internal StringOfset Convert() => tagConverter.Convert(text, position);
    }
}
