using Markdown.Styles;
using Markdown.Tokens;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string mdText)
        {
            if (string.IsNullOrWhiteSpace(mdText))
                return mdText;

            var tokens = RecognizeTokens(ref mdText);

            ApplyStyles(tokens, new List<Style> { new Italic(), new Bold() });

            return TextConverter.HTMLConverter().Convert(tokens);
        }

        public void RenderFile(string source, string dest)
        {
            var htmlpage = TextConverter.HTMLConverter().ConvertToHtmlPage(File.ReadAllLines(source).Select(l => Render(l)));
            File.WriteAllLines(dest, htmlpage);
        }

        private List<Token> RecognizeTokens(ref string mdText)
        {
            var tokens = new List<Token>();
            int i = 0;
            int wordBegin = 0;
            int wordLength = 0;
            while (i < mdText.Length)
            {
                if (SpecSymbol.IsSpecSymbol(mdText[i], out SpecSymbol specSymbol))
                {
                    if (wordLength > 0)
                    {
                        tokens.Add(new Word(mdText.Substring(wordBegin, wordLength)));
                        wordLength = 0;
                    }
                    tokens.Add(specSymbol);
                }
                else
                {
                    if (wordLength == 0)
                        wordBegin = i;
                    wordLength++;
                }

                i++;
            }
            if (wordLength > 0)
                tokens.Add(new Word(mdText.Substring(wordBegin, wordLength)));

            CorrectBackslashedTokens(tokens);

            return tokens;
        }

        private void CorrectBackslashedTokens(List<Token> tokens)
        {
            int i = 0;
            while (i < tokens.Count - 1)
            {
                if (tokens[i] is Backslash && tokens[i + 1] is Underline underline)
                {
                    tokens[i + 1] = new Word(underline.Symbol.ToString());
                    tokens.RemoveAt(i);
                }
                i++;
            }
        }

        private void ApplyStyles(List<Token> tokens, List<Style> styles)
        {
            styles.Sort((s1, s2) => s2.BeginingTokens.Length.CompareTo(s1.BeginingTokens.Length));

            var outerStyles = new Stack<Style>();
            int i = 0;
            while (i < tokens.Count)
            {
                if (tokens[i] is StyleEndToken)
                {
                    outerStyles.Pop();
                }
                else
                {
                    foreach (var knownStyle in styles)
                    {
                        if (CanBeApplied(knownStyle, outerStyles))
                        {
                            if (knownStyle.Apply(tokens, i))
                            {
                                outerStyles.Push(knownStyle);
                                break;
                            }
                        }
                    }
                }
                i++;
            }
        }

        private bool CanBeApplied(Style style, Stack<Style> outerStyles)
        {
            if (style is Italic && outerStyles.Any(s => s is Italic)) return false;
            if (style is Bold && outerStyles.Any(s => s is Bold)) return false;
            if (style is Bold && outerStyles.Any(s => s is Italic)) return false;

            return true;
        }
    }
}
