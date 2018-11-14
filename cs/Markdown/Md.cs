using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.String;

namespace Markdown
{
    public class Md
    {
        public string Render(string input)
        {
            var text = SplitList(input);
            var res = new StringBuilder();
            var textSeparatorStack = new Stack<Teg>();
            for (var i = 0; i < text.Count; i++)
            {
                if (!MarkupLanguage.IsKeyWords(text[i])) continue;

                if(i>0 && !text[i - 1].EndsWith(" ") && !IsNullOrEmpty(text[i - 1]))
                {
                    TextSeparator currentSep;
                    if (textSeparatorStack.Count <= 0 ||
                        (currentSep = textSeparatorStack.Peek().StartSeparator).separator != text[i])
                        continue;
                    var teg = Teg.CreateTegOnTextSeparator(currentSep);
                    if (textSeparatorStack.All(t => t.Rule.Check(teg)))// в текущем контексте это нормально, но если тег не может содержать сам себя работать не будет
                    {
                        text[currentSep.index] = $"<{teg}>";
                        text[i] = $"</{teg}>";
                    }
                    textSeparatorStack.Pop();
                }

                if (!text[i + 1].StartsWith(" ") && !IsNullOrEmpty(text[i + 1]))
                {
                    var currentSep = new TextSeparator(text[i], i);
                    textSeparatorStack.Push(Teg.CreateTegOnTextSeparator(currentSep));
                }
            }
            text.ForEach(t => res.Append(t));
            return res.ToString();
        }

        public static List<string> SplitList(string input)
        {
            var text = new StringBuilder(input);
            var splitedText = new List<string>();
            var currentToken = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var tt = MarkupLanguage.GetKeyWordsOnFirstLetter(text[i]);
                if (tt.Count > 0)
                {
                    var sovpadenie = false;
                    string keyWord = null;
                    foreach (var t in tt)
                    {
                        for (var indexPlus = 0; indexPlus < t.Length; indexPlus++)
                        {
                            if (i + indexPlus >= text.Length || text[i + indexPlus] != t[indexPlus])
                                break;
                            if (indexPlus + 1 != t.Length) continue;
                            sovpadenie = true;
                            keyWord = t;
                            i += indexPlus;
                        }
                        if (sovpadenie)
                            break;
                    }

                    if (!sovpadenie) continue;
                    splitedText.Add(currentToken.ToString());
                    splitedText.Add(keyWord);
                    currentToken = new StringBuilder();
                }
                else
                    currentToken.Append(text[i]);
            }
            splitedText.Add(currentToken.ToString());
            return splitedText;
        }
    }
}
