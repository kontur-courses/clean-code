using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sentence = System.Collections.Generic.IReadOnlyList<Markdown.Token>;

namespace Markdown
{
    public class Tokenizer
    {
        private SimpleTrieStateMachine tagSearcherMachine;

        private List<char> escapedSymbols;
        private int pointer;

        public IEnumerable<Sentence> Tokenize(string text, IEnumerable<string> keyWords)
        {
            InitializeTokenizer(keyWords, text);

            foreach (var sentence in text.Split('\n'))
            {
                yield return TokenizeWord(sentence);
                pointer += sentence.Length + 1;
            }
        }

        private List<Token> TokenizeWord(string sentence)
        {
            var tokenizeWord = new List<Token>();
            var emptyTagSymbols = new StringBuilder();


            for (var i = 0; i < sentence.Length; i++)
            {
                if (tagSearcherMachine.CanUpdateStates(sentence[i]))
                {
                    var tagToken = GetTagToken(sentence, i);
                    if (!tagToken.IsEmpty)
                    {
                        tokenizeWord.Add(tagToken);
                        i += tagToken.Value.Length - 1;
                    }
                    else
                    {
                        emptyTagSymbols.Append(sentence[i]);
                    }

                    continue;
                }

                if (char.IsWhiteSpace(sentence[i]))
                {
                    var wSToken = GetWhiteSpaceToken(sentence, i);

                    tokenizeWord.Add(wSToken);
                    i += wSToken.Value.Length - 1;
                    continue;
                }

                var textToken = GetTextToken(sentence, i, out var escapeCount, emptyTagSymbols);
                emptyTagSymbols.Clear();
                tokenizeWord.Add(textToken);
                i += textToken.Value.Length + escapeCount - 1;
            }

            return tokenizeWord;
        }

        private Token GetTextToken(string sentence, int index, out int escapeCount, StringBuilder textValue)
        {
            var tVLen = textValue.Length;
            escapeCount = 0;
            var isEscape = false;
            var i = index;

            while (HasContinueAddLetters(sentence, i, isEscape))
            {
                if (isEscape)
                {
                    if (escapedSymbols.Contains(sentence[i]))
                    {
                        textValue.Append(sentence[i]);
                        escapeCount++;
                    }
                    else
                    {
                        textValue.Append('\\');
                    }

                    i++;
                    isEscape = false;
                    continue;
                }

                if (sentence[i] == '\\')
                {
                    isEscape = true;
                    i++;
                    continue;
                }

                textValue.Append(sentence[i]);
                i++;
            }

            var token = new Token(index + pointer - tVLen, textValue.ToString(), TokenType.Text);
            return token;
        }

        private bool HasContinueAddLetters(string sentence, int i, bool isEscape)
        {
            return i < sentence.Length && !(tagSearcherMachine.CanUpdateStates(sentence[i])
                                            && !isEscape) && !char.IsWhiteSpace(sentence[i]);
        }

        private Token GetTagToken(string sentence, int index)
        {
            var tagValue = new StringBuilder();
            var i = index;
            var maxLenWord = "";
            var currentTrie = tagSearcherMachine.KeyWordsTrie;


            while (i < sentence.Length && tagSearcherMachine.CanUpdateStates(sentence[i], currentTrie))
            {
                currentTrie = tagSearcherMachine.UpdateStates(sentence[i], currentTrie);
                i++;
                if (currentTrie.IsTerminate)
                    maxLenWord = currentTrie.Value;
            }

            tagValue.Append(maxLenWord);


            var token = new Token(pointer + index, tagValue.ToString(), TokenType.Tag);

            return token;
        }

        private Token GetWhiteSpaceToken(string sentence, int index)
        {
            var wSTokenVal = new StringBuilder();
            var i = index;

            while (char.IsWhiteSpace(sentence[i]))
            {
                wSTokenVal.Append(sentence[i]);
                i++;
            }

            var token = new Token(index + pointer, wSTokenVal.ToString(), TokenType.WhiteSpace);

            return token;
        }

        private void InitializeTokenizer(IEnumerable<string> tags, string line)
        {
            tagSearcherMachine = new SimpleTrieStateMachine(tags);

            escapedSymbols = new List<char>();
            var escChars = tags
                .Select(tag => tag[0])
                .Union(new[] { '\\' });
            escapedSymbols.AddRange(escChars);

            pointer = 0;
        }
    }
}