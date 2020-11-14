using System;
using System.Collections.Generic;
using System.Text;

namespace TextFormatters
{
    public class Tokenizer
    {
        public Tokenizer(string input)
        {
            var lines = GetLines(input);
        }

        private List<Line> GetLines(string input)
        {
            var result = new List<Line>();
            var rawLines = input.Split('\n');
            var lineIndex = 0;
            var builder = new StringBuilder();
            while (lineIndex < rawLines.Length)
            {
                
            }
            foreach (var line in rawLines)
                result.Add(new Line(line, ));

            return result;
        }

        public IEnumerable<Token> Tokens()
        {
            yield break;
        }
    }
}
