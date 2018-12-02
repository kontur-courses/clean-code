using Markdown.TokenEssences;
namespace Markdown.TextProcessing
{
    public class EmTokenHandler : SimpleTokenHandler
    {
        public EmTokenHandler()
        {
            TokenAssociation = "_";
            StopChar = '_';
        }

        public override bool IsStartToken(string content, int position)
        {
            if (position == 0 && content.Length > 1 && content[position] == '_' && char.IsLetterOrDigit(content[position + 1]))
                return true;

            return position > 1 && position + 1 < content.Length &&
                   (char.IsSeparator(content[position - 1])|| char.IsControl(content[position - 1]) ||
                    content[position - 1] == '_' && IsNestedToken) &&
                   content[position] == '_' && char.IsLetterOrDigit(content[position + 1]);
        }

        public override bool IsStopToken(string content, int position)
        {
            if (position == content.Length - 1 && position > 0 &&
                content[position] == '_' && char.IsLetterOrDigit(content[position - 1]))
                return true;

            return position > 0 && position + 1 < content.Length &&
                   (char.IsSeparator(content[position + 1]) || char.IsControl(content[position + 1])  ||
                    (content[position + 1] == '_' && IsNestedToken) || char.IsPunctuation(content[position + 1])) &&
                   content[position] == '_' && char.IsLetterOrDigit(content[position - 1]);
        }

        public override TypeToken GetNextTypeToken(string content, int position)
        {
            return TypeToken.Simple;
        }

        public override ITokenHandler GetNextNestedToken(string content, int position)
        {
            return new SimpleTokenHandler();
        }
    }
}