using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class TagShield : ITagConverter
    {
        public StringOfset Convert(string text, int position)
        {
            if (Shield(text, position))
                return new StringOfset(text[position + 1].ToString(), 2);
            return new StringOfset(text[position].ToString(), 1);
        }

        private static bool Shield(string text, int position) =>
            position < text.Length - 1 && TagsAssociation.tagConverters.ContainsKey(text[position + 1].ToString());
    }
}
