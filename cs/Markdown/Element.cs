using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Element
    {
        public List<Element> NestedElements { get; set; }
        public string HtmlTag { get; set; }
        public string Data { get; set; }
        
        public string ToHtml() => throw new NotImplementedException();
    }
}