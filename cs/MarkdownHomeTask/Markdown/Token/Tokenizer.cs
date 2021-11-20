using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tokenizer
    {
        private SimpleTrieStateMachine tagSearcherMachine;

        private List<Token> tokens;
        private StringBuilder letterAggregator;
        private bool isEscape;
        private int escapeCount;
        private string text;
        private int pointer;

        public IReadOnlyList<Token> Tokenize(string line, IEnumerable<string> keyWords)
        {
            InitializeTokenizer(keyWords, line);

            while (pointer < text.Length)
            {
                if (char.IsWhiteSpace(text[pointer]))
                {
                    PullOutText();
                    pointer++;
                    continue;
                }

                if (isEscape)
                {
                    letterAggregator.Append(text[pointer++]);
                    isEscape = false;
                    continue;
                }

                if (text[pointer] == '\\')
                {
                    SetEscape();
                    continue;
                }

                if (tagSearcherMachine.IsUpdateStates(text[pointer]))
                {
                    PullOutText();
                    AddTagToken();
                    continue;
                }

                if (char.IsDigit(text[pointer]))
                {
                    PullOutText();
                    AddNumberToken();
                    continue;
                }

                letterAggregator.Append(text[pointer++]);
            }

            if (letterAggregator.Length > 0)
                PullOutText();

            return tokens;
        }

        private void InitializeTokenizer(IEnumerable<string> tags, string line)
        {
            text = line;
            tagSearcherMachine = new SimpleTrieStateMachine(tags);
            tokens = new List<Token>();
            letterAggregator = new StringBuilder();
            isEscape = false;
            escapeCount = 0;
            pointer = 0;
        }

        private void SetEscape()
        {
            pointer++;
            escapeCount++;
            isEscape = true;
        }

        
        private void PullOutText()
        {
            if (pointer < text.Length && !char.IsWhiteSpace(text[pointer]) && letterAggregator.Length == 0)
            {
                return;
            }

            var start = pointer - letterAggregator.Length - escapeCount;
            var textToken = new Token(start, letterAggregator.ToString(), TokenType.Text);

            tokens.Add(textToken);
            letterAggregator.Clear();

            escapeCount = 0;
        }

        private void AddNumberToken()
        {
            var tokenValue = new StringBuilder();
            tokenValue.Append(text[pointer]);

            var i = pointer + 1;

            while (i < text.Length && char.IsDigit(text[i]))
            {
                tokenValue.Append(text[i]);
                i++;
            }

            if (tokenValue.Length == 0) return;
            var token = new Token(pointer, tokenValue.ToString(), TokenType.Number);
            tokens.Add(token);

            pointer += tokenValue.Length;
        }

        private void AddTagToken()
        {
            var i = pointer + 1;

            while (i < text.Length && tagSearcherMachine.IsUpdateStates(text[i]))
            {
                i++;
            }

            var tokenValue = tagSearcherMachine.GetMaxFoundWord();

            if (tokenValue == null)
            {
                letterAggregator.Append(text.Substring(pointer, i));
                pointer += i;
                return;
            }

            var token = new Token(pointer, tokenValue, TokenType.Tag);
            tokens.Add(token);

            pointer += tokenValue.Length;
        }
    }
}