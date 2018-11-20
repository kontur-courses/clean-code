using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class StrongTokenHandler : SimpleTokenHandler
    {
        public StrongTokenHandler()
        {
            TokenAssociation = "__";
            IsStopChar = stopChar => stopChar == '_';
        }

        public override bool IsStartToken(string content, int position)
        {
            if (position == 0 && content.Length > 2 && content[position] == '_' && content[position + 1] == '_' &&
                char.IsLetterOrDigit(content[position + 2]))
                return true;

            return position > 1 && position + 2 < content.Length &&
                   char.IsSeparator(content[position - 1]) &&
                   content[position] == '_' && content[position + 1] == '_' &&
                   (char.IsLetterOrDigit(content[position + 2]) || content[position + 2] == '_');
        }

        public override bool IsStopToken(string content, int position)
        {
            if (position > 0 && position + 1 == content.Length - 1 &&
                content[position] == '_' && content[position + 1] == '_' &&
                (char.IsLetterOrDigit(content[position + -1]) || content[position + -1] == '_'))
                return true;

            return position > 0 && position + 2 < content.Length &&
                   (char.IsSeparator(content[position + 2]) || char.IsPunctuation(content[position + 2])) &&
                   content[position] == '_' && content[position + 1] == '_' &&
                   (char.IsLetterOrDigit(content[position - 1]) || content[position - 1] == '_');
        }

        public override TypeToken GetNextTypeToken(string content, int position)
        {
            return TypeToken.Simple;
        }

        public override bool IsNestedToken(string content, int position)
        {
            var emToken = new EmTokenHandler();
            return emToken.IsStartToken(content, position);
        }

        public override ITokenHandler GetNextNestedToken(string content, int position)
        {
            return new EmTokenHandler();
        }
    }
}