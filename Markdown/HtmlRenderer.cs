using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;
using Markdown.Shell;
using Markdown.Tokenizer;

namespace Markdown
{
    public class HtmlRenderer
    {
        private readonly List<Attribute> commonAttributes;

        public HtmlRenderer(List<Attribute> commonAttributes)
        {
            this.commonAttributes = commonAttributes;
        }

        private static readonly Dictionary<Type, string> HtmlTags = new Dictionary<Type, string>
        {
            { typeof(SingleUnderline),"em"},
            { typeof(DoubleUnderline), "strong" },
            { typeof(UrlShell), "a" }

        };

        private static readonly Dictionary<AttributeType, string> HtmlAttributes = new Dictionary<AttributeType, string>
        {
            {AttributeType.Style, "style" },
            {AttributeType.Url, "href" }
        };

        public string Render(Token token)
        {
            if (!token.HasShell())
            {
                return token.Text;
            }
            var attributes = token.Attributes.Concat(commonAttributes).ToList();
            var tag = HtmlTags[token.Shell.GetType()];
            return CreateTag(token.Text, tag, attributes);

        }

        public string CreateTag(string content, string tag, List<Attribute> attributes)
        {
            var attributesText = Concat(attributes.Select(ConvertToString));
            return $"<{tag}{attributesText}>{content}</{tag}>";
        }

        private string ConvertToString(Attribute attribute)
        {
            return $" {HtmlAttributes[attribute.Type]}={attribute.Value}";
        }
    }
}
