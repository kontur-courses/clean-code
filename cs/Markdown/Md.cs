using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public static string Render(string sourceText)
        {
            var currentText = new StringBuilder(sourceText);
            var tagsPair = TagsParser.GetTagsPair(currentText, MdTags.GetAllTags());
            var result = TagsConverter.ConvertToNewSpecifications(currentText, MdTags.GetAllTags(),HTMLTags.GetAllTags(), tagsPair);
            return result;
        }
    }
}
