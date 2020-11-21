using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models.ConvertOptions;
using Markdown.Models.ConvertOptions.ConvertRules;
using Markdown.Models.ConvertOptions.UnionRules;

namespace Markdown.Models.Converters
{
    internal class MdToHtmlConverter : IConverter
    {
        private readonly ConvertingOptions options;
        public IConvertRule NewLineRule => options.NewLineRule;

        public MdToHtmlConverter(ConvertingOptions options)
        {
            this.options = options;
        }

        public string Convert(IEnumerable<IEnumerable<ITaggedToken>> textTokens)
        {
            var result = new StringBuilder();

            IUnionRule opened = null;
            var isFirst = true;
            foreach (var lineTokens in textTokens)
            {
                if (TryGetUnionRule(lineTokens.First(), out var unionRule))
                {
                    if (opened == null)
                        result.Append(unionRule.ToUnionWith.Opening);

                    result.Append(ConvertLine(lineTokens, false));
                    opened = unionRule;
                }
                else
                {
                    if (opened != null)
                        result.Append(opened.ToUnionWith.Closing);
                    result.Append(ConvertLine(lineTokens, !isFirst));
                }
                isFirst = false;
            }

            if (opened != null)
                result.Append(opened.ToUnionWith.Closing);

            return result.ToString();
        }

        private string ConvertLine(IEnumerable<ITaggedToken> tokens,
            bool withNewLine = true)
        {
            var result = new StringBuilder();
            foreach (var taggedToken in tokens)
                result.Append(ConvertToken(taggedToken));

            var newLineTag = options.NewLineRule.To;

            return withNewLine ? $"{newLineTag.Opening}{result}{newLineTag.Closing}" :
                result.ToString();
        }

        private string ConvertToken(ITaggedToken token)
        {
            var convertTag = TryGetConvertRule(token, out var convertRule) ?
                convertRule.To : token.Tag;

            var tokenWithInnersValue = token.InnerTokens.Count == 0 ?
                token.Value : string.Join("", token.InnerTokens.Select(ConvertToken));

            return $"{convertTag.Opening}{tokenWithInnersValue}{convertTag.Closing}";
        }

        private bool TryGetUnionRule(ITaggedToken token, out IUnionRule rule)
        {
            rule = null;
            if (TryGetConvertRule(token, out var convertRule))
                rule = options.UnionRules
                    .FirstOrDefault(r => convertRule.To.Equals(r.Element));

            return rule != null;
        }

        private bool TryGetConvertRule(ITaggedToken token, out IConvertRule rule)
        {
            var convertRule = options.ConvertRules
                .FirstOrDefault(r => token.Tag.Equals(r.From));
            rule = convertRule;
            return convertRule != null;
        }
    }
}
