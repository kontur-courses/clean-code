using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class MarkdownParser : IMarkdownParser
    {
        public IEnumerable<string> ParseMarkdownLexemes(string markdown)
        {
            var lexemes = ParseLexemes(markdown);

            // Вот здесь есть проблема, на самом деле не очень понятно
            // зачем я склеиваю Italic в Bold в обратном порядке
            // как решать такую проблему? Комментарии?
            lexemes = ReverseMerge(lexemes, ("_", "_"));

            // На самом деле нам без разницы в каком порядке клеить "#" и " ",
            // поэтому решил просто заюзать тот же метод, что и для склейки Italic в Bold
            lexemes = ReverseMerge(lexemes, ("#", " "));

            return lexemes;
        }

        private static List<string> ParseLexemes(string markdown)
        {
            var lexemes = new List<string>();
            var lexeme = new StringBuilder();
            foreach (var symbol in markdown)
            {
                if (char.IsLetter(symbol))
                {
                    lexeme.Append(symbol);
                }
                else
                {
                    if (lexeme.Length > 0)
                    {
                        lexemes.Add(lexeme.ToString());
                    }

                    lexeme = new StringBuilder();
                    lexemes.Add(symbol.ToString());
                }
            }

            if (lexeme.Length > 0)
            {
                lexemes.Add(lexeme.ToString());
            }

            return lexemes;
        }

        // Вообще я не знаю куда отправить этот метод, в токенайзер, сюда или вообще создать какую-то прослойку,
        // но все-таки я думаю, что раз уж я решил назвать метод ParseLexemes, а класс MarkdownParser,
        // то и возвращать он должен готовые лексемы. Не думаю, что склейка это ответственность токенайзера.
        private static List<string> ReverseMerge(IReadOnlyList<string> lexemes,
            (string firstLexeme, string secondLexeme) lexemesToMerge)
        {
            var merged = new List<string>();

            var (firstLexeme, secondLexeme) = lexemesToMerge;

            var mergedLexemes = firstLexeme + secondLexeme;

            for (var i = lexemes.Count - 1; i >= 0; --i)
            {
                if (i > 0 && lexemes[i] == secondLexeme && lexemes[i - 1] == firstLexeme)
                {
                    merged.Add(mergedLexemes);
                    --i;
                }
                else
                {
                    merged.Add(lexemes[i]);
                }
            }

            merged.Reverse();

            return merged;
        }
    }
}