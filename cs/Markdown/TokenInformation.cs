using System.Collections.Generic;

namespace Markdown
{
    public class TokenInformation
    {
        public bool EndIsNewLine;
        public bool IsPaired;
        public string Symbol;
        public string Tag;

        public override bool Equals(object obj)
        {
            var information = obj as TokenInformation;
            return information != null &&
                   EndIsNewLine == information.EndIsNewLine &&
                   IsPaired == information.IsPaired &&
                   Symbol == information.Symbol &&
                   Tag == information.Tag;
        }

        public override int GetHashCode()
        {
            var hashCode = 480016742;
            hashCode = hashCode * -1521134295 + EndIsNewLine.GetHashCode();
            hashCode = hashCode * -1521134295 + IsPaired.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Symbol);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tag);
            return hashCode;
        }
    }
}