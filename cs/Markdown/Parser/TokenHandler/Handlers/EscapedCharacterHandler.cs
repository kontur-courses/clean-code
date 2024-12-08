using Markdown.Token;

namespace Markdown.Parser.TokenHandler.Handlers
{
    public class EscapedCharacterHandler : BaseTokenHandler
    {
        public EscapedCharacterHandler() : base(new Delimiter(@"\", @"\", TokenType.Escaped))
        {
        }

        public override bool TryHandle(ParsingContext context, out Token.Token token, out int skip)
        {
            token = null;
            skip = 0;

            var text = context.Text;
            var position = context.Position;
            
            if (!IsMatch(text, position, Delimiter.Opening))
                return false;
            
            if (position + 1 >= text.Length)
                return false;
            
            var escapedChar = text[position + 1];
            token = Token.Token.CreateText(escapedChar.ToString(), position);
            skip = 2; 

            return true;
        }
        
    }
}