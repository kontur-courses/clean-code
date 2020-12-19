using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string inputLine)
        {
            var mainToken = CreateMainToken(inputLine);
            var inputLineBuilder = new StringBuilder(inputLine);
            new TokenParser().Parse(mainToken, inputLineBuilder);
            return new TokenWriter(inputLineBuilder.ToString()).Write(mainToken);
        }

        private Token CreateMainToken(string line)
        {
            return new Token(0, line.Length, null, TokenType.Simple);
        }
    }
}