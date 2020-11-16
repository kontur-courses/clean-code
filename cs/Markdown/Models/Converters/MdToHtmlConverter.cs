using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models.ConvertOptions;

namespace Markdown.Models.Converters
{
    internal class MdToHtmlConverter : IConverter
    {
        private readonly ConvertingOptions options;

        public MdToHtmlConverter(ConvertingOptions options)
        {
            this.options = options;
        }

        public string Convert(IEnumerable<ITaggedToken> tokens, bool withNewLine = true)
        {
            var result = new StringBuilder();

            foreach (var taggedToken in tokens)
                result.Append(ConvertToken(taggedToken));

            var newLineTag = options.NewLine;

            return withNewLine ? $"{newLineTag.Opening}{result}{newLineTag.Closing}" :
                result.ToString();
        }

        private string ConvertToken(ITaggedToken token)
        {
            var convertRule = options.ConvertRules.FirstOrDefault(rule => token.Tag.Equals(rule.From));
            var convertTag = convertRule != null ? convertRule.To : token.Tag;

            var tokenWithInnersValue = token.InnerTokens.Count == 0 ?
                token.Value : string.Join("", token.InnerTokens.Select(ConvertToken));

            return $"{convertTag.Opening}{tokenWithInnersValue}{convertTag.Closing}";
        }
    }
}
