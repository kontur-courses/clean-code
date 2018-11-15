using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.String;

namespace Markdown
{
    public class Md
    {
        private List<string> text;
        private Stack<Teg> textSeparatorStack = new Stack<Teg>();
        public string Render(string input)
        {
            text = SplitList(input);
            var res = new StringBuilder();
            for (var index = 0; index < text.Count; index++)
            {
                if (!MarkupLanguage.IsKeyWords(text[index])) continue;

                int number;
                if (int.TryParse(text[index - 1], out number) || int.TryParse(text[index + 1], out number))
                    continue;

                if (index>0 && !text[index - 1].EndsWith(" ") && !IsNullOrEmpty(text[index - 1]))
                {
                    ClosingTeg(index);
                }

                if (!text[index + 1].StartsWith(" ") && !IsNullOrEmpty(text[index + 1]))
                {
                    OpeningTeg(index);
                }
            }
            text.ForEach(t => res.Append(t));
            return res.ToString();
        }

        private void OpeningTeg(int index)
        {
            var currentSep = new TextSeparator(text[index], index);
            textSeparatorStack.Push(Teg.CreateTegOnTextSeparator(currentSep));
        }

        private bool ClosingTeg(int index)
        {
            TextSeparator currentSep;
            if (textSeparatorStack.Count <= 0 ||
                (currentSep = textSeparatorStack.Peek().StartSeparator).separator != text[index])
                return false;
            var teg = Teg.CreateTegOnTextSeparator(currentSep);
            if (textSeparatorStack.All(t => t.Rule.Check(teg)))// в текущем контексте это нормально, но если тег не может содержать сам себя работать не будет
            {
                text[currentSep.index] = $"<{teg}>";
                text[index] = $"</{teg}>";
            }
            textSeparatorStack.Pop();
            return true;
        }

        public static List<string> SplitList(string input)
        {
            var text = new StringBuilder(input);
            var splitedText = new List<string>();
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
                        splitedText.Add(currentToken.ToString());
                        splitedText.Add(possibleKeyWord);
                        currentToken.Clear();
                        break;
                    }
                }
                else
                    currentToken.Append(text[i]);
            }
            splitedText.Add(currentToken.ToString());
            return splitedText;
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
