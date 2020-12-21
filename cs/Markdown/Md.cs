using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string inputLine)
        {
            var mainToken = CreateMainToken(inputLine);
            new TokenParser(inputLine).Parse(mainToken);
            return new TokenWriter(inputLine).Write(mainToken);
        }

        private Token CreateMainToken(string line)
        {
            return new Token(0, line.Length, null, TokenType.Simple);
        }
    }
}