
namespace Markdown.Types
{
    public class EmToken : SimpleToken
    {
        public EmToken(int position, int length, string value)
        {
            Position = position;
            Length = length;
            Value = value;
            TypeToken = TypeToken.Em;
            TokenAssociation = "_";
            IsStopChar = stopChar => stopChar == '_';
        }

        public EmToken()
        {
            TypeToken = TypeToken.Em;
            Value = "";
            TokenAssociation = "_";
            IsStopChar = stopChar => stopChar == '_';
        }

        public override bool IsStartToken(string content, int position)
        {
            if (position == 0 && content.Length > 1 && content[position] == '_' && char.IsLetterOrDigit(content[position + 1]))
                return true;

            return position > 0 && position + 1 < content.Length && (char.IsSeparator(content[position - 1]) || content[position - 1] == '_') &&
                   content[position] == '_' && char.IsLetterOrDigit(content[position + 1]);
        }

        public override bool IsStopToken(string content, int position)
        {
            if (position == content.Length - 1 && position > 0 &&
                content[position] == '_' && char.IsLetterOrDigit(content[position - 1]))
                return true;

            return position > 0 && position + 1 < content.Length &&
                   (char.IsSeparator(content[position + 1]) || content[position + 1] == '_' || char.IsSeparator(content[position + 1])) &&
                   content[position] == '_' && char.IsLetterOrDigit(content[position - 1]);
        }

        public override TypeToken GetNextTypeToken(string content, int position)
        {
            return TypeToken.Simple;
        }

        public override bool IsNestedToken(string content, int position)
        {
            return false;
        }

        public override IToken GetNextNestedToken(string content, int position)
        {
            return new SimpleToken();
        }
    }
}