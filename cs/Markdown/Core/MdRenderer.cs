using System.Collections.Generic;
using Markdown.Core.Infrastructure;
using Markdown.Core.Normalizer;
using Markdown.Core.Readers;
using Markdown.Core.Translators;

namespace Markdown.Core
{
    public class MdRenderer
    {
        public string Render(string source)
        {
            var tokens = new ParagraphTokenReader().ReadTokens(source);

            var ignoreInsideRules = new List<IgnoreInsideRule>()
            {
                new IgnoreInsideRule(
                    new List<TagInfo>() {TagsUtils.GetTagInfoByTagName("em")},
                    TagsUtils.GetTagInfoByTagName("strong"))
            };

            tokens = new MdNormalizer().NormalizeTokens(tokens, ignoreInsideRules);

            return new MdToHtmlTranslator().TranslateTokensToHtml(tokens);
        }
    }
}