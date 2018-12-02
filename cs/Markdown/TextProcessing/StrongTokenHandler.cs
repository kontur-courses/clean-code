using Markdown.TokenEssences;

namespace Markdown.TextProcessing
{
    public class StrongTokenHandler : SimpleTokenHandler
    {
        public StrongTokenHandler()
        {
            TokenAssociation = "__";
            StopChar = '_';
            IsNestedToken = true;
        }

        public override bool IsStartToken(string content, int position)
        {
            if (position == 0 && content.Length > 3 && content[position] == '_' && content[position + 1] == '_' &&
                (char.IsLetterOrDigit(content[position + 2]) || (char.IsLetterOrDigit(content[position + 3]) && content[position + 2] == '_')))
                return true;

            return position > 0 && position + 3 < content.Length &&
                   (char.IsSeparator(content[position - 1]) || char.IsControl(content[position - 1])) &&
                   content[position] == '_' && content[position + 1] == '_' &&
                   (char.IsLetterOrDigit(content[position + 2]) ||
                    content[position + 2] == '_' && char.IsLetterOrDigit(content[position + 3]));
        }

        public override bool IsStopToken(string content, int position)
        {
            if (position > 0 && position + 1 == content.Length - 1 &&
                content[position] == '_' && content[position + 1] == '_' &&
                (char.IsLetterOrDigit(content[position - 1]) || content[position - 1] == '_'))
                return true;

            return position > 0 && position + 2 < content.Length &&
                   (char.IsSeparator(content[position + 2]) || char.IsPunctuation(content[position + 2]) ||
                    char.IsControl(content[position + 2])) &&
                   content[position] == '_' && content[position + 1] == '_' &&
                   (char.IsLetterOrDigit(content[position - 1]) || content[position - 1] == '_');
        }

        public override TypeToken GetNextTypeToken(string content, int position)
        {
            return TypeToken.Simple;
        }

        public override bool ContainsNestedToken(string content, int position)
        {
            var emToken = new EmTokenHandler {IsNestedToken = true};
            return emToken.IsStartToken(content, position);
        }

        public override ITokenHandler GetNextNestedToken(string content, int position)
        {
            return new EmTokenHandler();
        }
    }
}