using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownDocument.Inline;

namespace Markdown
{
    public static class Lexer
    {
        public static List<Lexeme> ExtractLexemes(string text)
        {
            var lexemes = new Stack<Lexeme>();
            foreach (var lexeme in text.Select(c => Lexeme.CreateFromChar(c)))
            {
                if (lexeme.IsPunctuation() && lexemes.Count > 0 && lexemes.Peek().Value == "\\")
                {
                    lexemes.Pop();
                    lexeme.Escaped = true;
                }
                lexemes.Push(lexeme);
            }

            var lexemesList = lexemes.Reverse().ToList();
            lexemesList = EscapeUnderlinesInTextWithNumbers(lexemesList);
            return lexemesList;
        }

        private static List<Lexeme> EscapeUnderlinesInTextWithNumbers(List<Lexeme> lexemes)
        {
            for (int i = 0; i < lexemes.Count; ++i)
            {
                if (!lexemes[i].IsDigit()) continue;
                for (var j = i - 1; j >= 0 && !lexemes[j].IsWhitespace(); --j)
                {
                    if (lexemes[j].Value == "_")
                        lexemes[j].Escaped = true;
                }
                    
                for (var j = i + 1; j < lexemes.Count && !lexemes[j].IsWhitespace(); ++j)
                {
                    if (lexemes[j].Value == "_")
                        lexemes[j].Escaped = true;
                }
            }

            return lexemes;
        }
    }
}