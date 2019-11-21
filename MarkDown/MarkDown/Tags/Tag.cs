using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TagParsers
{
    public abstract class Tag
    {
        public abstract string OpeningHtmlTag { get; }
        public abstract string ClosingHtmlTag { get; }
        public abstract string MdTag { get; }

        public override bool Equals(object obj)
        {
            return obj is Tag tag &&
                   OpeningHtmlTag == tag.OpeningHtmlTag &&
                   ClosingHtmlTag == tag.ClosingHtmlTag &&
                   MdTag == tag.MdTag;
        }

        public override int GetHashCode()
        {
            var hashCode = OpeningHtmlTag.GetHashCode();
            hashCode *= ClosingHtmlTag.GetHashCode();
            hashCode *= MdTag.GetHashCode();
            return hashCode;
        }
    }
}