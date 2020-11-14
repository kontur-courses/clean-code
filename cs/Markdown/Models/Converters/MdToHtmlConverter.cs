using System;
using System.Collections.Generic;
using Markdown.Models.ConvertingRules;

namespace Markdown.Models.Converters
{
    internal class MdToHtmlConverter : IConverter
    {
        private readonly IEnumerable<IConvertRule> convertRules;

        public MdToHtmlConverter(IEnumerable<IConvertRule> convertRules)
        {
            this.convertRules = convertRules;
        }

        public string Convert(IEnumerable<TaggedToken> tokens)
        {
            throw new NotImplementedException();
        }

        private string ConvertToken(TaggedToken token)
        {
            throw new NotImplementedException();
        }
    }
}
