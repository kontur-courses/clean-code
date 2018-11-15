using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.String;

namespace Markdown
{
    public class Md
    {
        private List<string> splittedText = new List<string>();
        private readonly Stack<Teg> textSeparatorStack = new Stack<Teg>();
        public string Render(string input)
        {
            splittedText = SplitList(input);
            var result = new StringBuilder();
            for (var index = 0; index < splittedText.Count; index++)
            {
                if (!MarkupLanguage.IsKeyWords(splittedText[index])) continue;

                if (int.TryParse(splittedText[index - 1], out _) || int.TryParse(splittedText[index + 1], out _)) // todo highlight
                    continue;

                if (splittedText[index] == MarkupLanguage.Screening) // todo highlight
                {
                    splittedText.RemoveAt(index++);
                    continue;
                }

                if (index > 0 && !splittedText[index - 1].EndsWith(" ") && !IsNullOrEmpty(splittedText[index - 1])) ClosingTeg(index);
                else if (!splittedText[index + 1].StartsWith(" ") && !IsNullOrEmpty(splittedText[index + 1])) OpeningTeg(index);
            }
            splittedText.ForEach(t => result.Append(t));
            return result.ToString();
        }

        private void OpeningTeg(int index)
        {
            var currentSep = new TextSeparator(splittedText[index], index);
            textSeparatorStack.Push(Teg.CreateTegOnTextSeparator(currentSep));
        }

        private bool ClosingTeg(int index)
        {
            TextSeparator currentSep;
            if (textSeparatorStack.Count <= 0 ||
                (currentSep = textSeparatorStack.Peek().StartSeparator).Separator != splittedText[index])
                return false;
            var teg = Teg.CreateTegOnTextSeparator(currentSep);
            if (textSeparatorStack.All(t => t.TegRule.Check(teg))) // todo в текущем контексте это нормально, но если тег не может содержать сам себя работать не будет
            {
                splittedText[currentSep.Index] = $"<{teg}>";
                splittedText[index] = $"</{teg}>";
            }
            textSeparatorStack.Pop();
            return true;
        }

        public List<string> SplitList(string input)
        {
            var text = new StringBuilder(input);
            var currentToken = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var possibleKeyWords = MarkupLanguage.GetKeyWordsOnFirstLetter(text[i]);
                if (possibleKeyWords.Count > 0)
                {
                    foreach (var possibleKeyWord in possibleKeyWords)
                    {
                        if (!CompareStringBuilderPartWithString(possibleKeyWord, text, i)) continue;
                        i += possibleKeyWord.Length-1;
                        splittedText.Add(currentToken.ToString());
                        splittedText.Add(possibleKeyWord);
                        currentToken.Clear();
                        break;
                    }
                }
                else
                    currentToken.Append(text[i]);
            }
            splittedText.Add(currentToken.ToString());
            return splittedText;
        }

        private static bool CompareStringBuilderPartWithString(string possibleKeyWord, StringBuilder text, int index)
        {
            return possibleKeyWord
                .TakeWhile((t, indexPlus) => index + indexPlus < text.Length && text[index + indexPlus] == t)
                .Where((t, indexPlus) => indexPlus + 1 == possibleKeyWord.Length)
                .Any();
        }
    }
}
