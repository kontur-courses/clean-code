namespace Markdown
{
    public class Md
    {
        public string Render(string inputLine)
        {
            var mainToken = CreateMainToken(inputLine);
            var tokenReader = new TokenReader(inputLine);
            var lineWithoutEscapedChars = tokenReader.Read(mainToken);
            var tokenWriter = new TokenWriter(lineWithoutEscapedChars);
            tokenWriter.Write(mainToken);
            var resultLine = tokenWriter.GetString();
            return resultLine;
        }

        private Token CreateMainToken(string line)
        {
            return new Token(0, line.Length);
        }
    }
}