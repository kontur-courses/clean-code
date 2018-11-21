using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDown.TagTypes
{
    public abstract class TagType
    {
        public string OpeningSymbol { get;}
        public string ClosingSymbol { get; }
        public string HtmlTag { get; }
        public Parameter Parameter { get; }
        private readonly IEnumerable<Type> unavailableNestedTagTypes;
        
        protected TagType(string openingSymbol, string closingSymbol, string htmlTag, IEnumerable<Type> unavailableNestedTagTypes, Parameter parameter = null)
        {
            OpeningSymbol = openingSymbol;
            ClosingSymbol = closingSymbol;
            HtmlTag = htmlTag;
            this.unavailableNestedTagTypes = unavailableNestedTagTypes;
            Parameter = parameter;
        }

        public List<TagType> GetNestedTagTypes(IEnumerable<TagType> tagTypes)
        {
            return tagTypes.Where(tagType => !unavailableNestedTagTypes.Contains(tagType.GetType())).ToList();
        }

        public string ToHtml(string text, string param = "") => 
            Parameter == null ? $"<{HtmlTag}>{text}</{HtmlTag}>" : 
                $@"<{HtmlTag} {Parameter.Name}=""{param}"">{text}</{HtmlTag}>";
    }
}
