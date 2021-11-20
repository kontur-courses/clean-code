using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Tokenizer
    {
        private SimpleTrieStateMachine simpleTrieStateMachine;

        public IReadOnlyList<Token> Tokenize(string line, IEnumerable<string> tags)
        {
            simpleTrieStateMachine = new SimpleTrieStateMachine(tags);

            var tokens = new List<Token>();
            var text = new StringBuilder();
            var isEscape = false;
            var escapeCount = 0;

            for (var pointer = 0; pointer < line.Length;)
            {
                if (char.IsWhiteSpace(line[pointer]))
                {
                    escapeCount = AddText(pointer, text, escapeCount, tokens, true);
                    pointer++;
                    continue;
                }

                if (isEscape)
                {
                    text.Append(line[pointer++]);
                    isEscape = false;
                    continue;
                }

                if (line[pointer] == '\\')
                {
                    pointer++;
                    escapeCount++;
                    isEscape = true;
                    continue;
                }

                if (simpleTrieStateMachine.UpdateStates(line[pointer]))
                {
                    escapeCount = AddText(pointer, text, escapeCount, tokens);

                    var tag = GetTag(line, pointer);
                    var token = new Token(pointer, tag, TokenType.Tag);
                    tokens.Add(token);
                    pointer += tag.Length;
                    continue;
                }

                if (char.IsDigit(line[pointer]))
                {
                    escapeCount = AddText(pointer, text, escapeCount, tokens);

                    var number = GetNumber(line, pointer);
                    var numberToken = new Token(pointer, number, TokenType.Number);
                    tokens.Add(numberToken);
                    pointer += number.Length;
                    continue;
                }

                text.Append(line[pointer++]);
            }

            AddText(line.Length, text, escapeCount, tokens);

            return tokens;
        }

        private static int AddText(int pointer, StringBuilder text, int escapeCount, ICollection<Token> tokens,
            bool isWhiteSpace = false)
        {
            if (!isWhiteSpace && text.Length == 0)
            {
                return escapeCount;
            }

            var start = pointer - text.Length - escapeCount;
            var textToken = new Token(start, text.ToString(), TokenType.Text);

            tokens.Add(textToken);
            text.Clear();

            return 0;
        }

        private static string GetNumber(string line, int pointer)
        {
            var number = new StringBuilder();
            number.Append(line[pointer]);
            while (char.IsDigit(line[++pointer]))
            {
                number.Append(line[pointer]);
            }

            return number.ToString();
        }

        private string GetTag(string line, int pointer)
        {
            var tag = new StringBuilder();
            tag.Append(line[pointer]);

            while (simpleTrieStateMachine.UpdateStates(line[++pointer]))
            {
                tag.Append(line[pointer]);
            }

            return tag.ToString();
        }
    }
}