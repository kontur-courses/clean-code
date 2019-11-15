using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework.Constraints;

namespace MarkDown.TagParsers
{
    public class EmTagParser : TagParser
    {
        public override string OpeningHtmlTag { get; } = "<em>";
        public override string ClosingHtmlTag { get; } = @"</em>";
        public override string MdTag { get; } = "_";
    }
}