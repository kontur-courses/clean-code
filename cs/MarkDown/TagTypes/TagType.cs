using System.Collections.Generic;
using System.Linq;

namespace MarkDown.TagTypes
{
    public abstract class TagType
    {
        public string SpecialSymbol { get;}
        public string HtmlTag { get; }
        private readonly IEnumerable<TagType> availableNestedTagTypes;

        protected TagType(string specialSymbol, string htmlTag, IEnumerable<TagType> availableNestedTagTypes)
        {
            SpecialSymbol = specialSymbol;
            this.HtmlTag = htmlTag;
            this.availableNestedTagTypes = availableNestedTagTypes;
        }

        public bool IsInAvailableNestedTagTypes(TagType tagType) => availableNestedTagTypes.Any(t => t.GetType() == tagType.GetType());

        public string ToHtml(string text) => $"<{HtmlTag}>{text}</{HtmlTag}>";
    }
}
