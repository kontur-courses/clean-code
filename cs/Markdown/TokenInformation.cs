namespace Markdown
{
    public class TokenInformation
    {
        public int CountOfSpaces;
        public bool EndIsNewLine;
        public bool IsPaired;
        public string Symbol;
        public string Tag;

        public override bool Equals(object obj)
        {
            var information = obj as TokenInformation;
            return information != null &&
                   CountOfSpaces == information.CountOfSpaces &&
                   EndIsNewLine == information.EndIsNewLine &&
                   IsPaired == information.IsPaired &&
                   Symbol == information.Symbol &&
                   Tag == information.Tag;
        }
    }
}