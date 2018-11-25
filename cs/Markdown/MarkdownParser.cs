using System.Collections.Generic;
using System.Linq;
using Markdown.Types;

namespace Markdown
{
    public class MarkdownParser
    {
        private readonly string markdownInput;
        public Token CurrentToken;
        private readonly Specification specification = new Specification();

        public MarkdownParser(string markdownInput)
        {
            this.markdownInput = markdownInput;
            CurrentToken = new Token();
        }

        public Token GetTokens()
        {
            var searcher = new StringSearcher();
            var allKeyWords = new HashSet<string>();
            var mdDelimiters = specification.MdDelimiters;
            allKeyWords.UnionWith(specification.KeyWords);
            allKeyWords.UnionWith(specification.ProhibitionСharacters);
            allKeyWords.UnionWith(mdDelimiters);
            var lexemes = searcher.SplitBySubstrings(allKeyWords, markdownInput);

            for (var lexemeIndex = 0; lexemeIndex < lexemes.Count; lexemeIndex++)
            {
                var lexeme = lexemes[lexemeIndex];

                if (lexeme.Value == "\\")
                {
                    lexemeIndex = Escape(lexemes, lexemeIndex);
                    continue;
                }

                if (mdDelimiters.Contains(lexeme.Value) &&
                    SurroundedByDigits(lexemes, lexemeIndex))
                {
                    CurrentToken.AddText(lexeme.Value);
                    continue;
                }

                if (!mdDelimiters.Contains(lexeme.Value))
                {
                    CurrentToken.AddText(lexeme.Value);
                }

                else
                {
                    AddNewDelimiter(lexeme);
                }
            }

            return CurrentToken.RootToken;
        }

        private void AddNewDelimiter(Substring lexeme)
        {
            var canBeClosing = specification.CanBeClosing(lexeme, markdownInput);
            var canBeStarting = specification.CanBeStarting(lexeme, markdownInput);
            var delimiter = new Delimiter(lexeme.Value, lexeme.Index, canBeClosing, canBeStarting);
            if (delimiter.CanBeClosing)
            {
                var closed = TryCloseToken(delimiter, out var closedToken);
                if (closed)
                {
                    ResolveToken(closedToken);
                    CurrentToken = closedToken.ParentToken;
                    return;
                }
            }

            if (delimiter.CanBeStarting)
            {
                var newToken = new Token(delimiter);
                CurrentToken.AddToken(newToken);
                CurrentToken = newToken;
            }
            else
            {
                CurrentToken.AddText(delimiter.Value);
            }
        }


        private int Escape(List<Substring> substrings, int lexemeIndex)
        {
            var mdDelimiters = new HashSet<string>(specification.MdToTokenTypes.Keys);
            var charactersToEscape = new HashSet<string>();
            charactersToEscape.UnionWith(mdDelimiters);
            charactersToEscape.UnionWith(specification.KeyWords);
            var result = lexemeIndex;
            if (lexemeIndex + 1 < substrings.Count &&
                charactersToEscape.Contains(substrings[lexemeIndex + 1].Value))
            {
                CurrentToken.AddText(substrings[lexemeIndex + 1].Value);
                result += 1;
            }
            else
            {
                CurrentToken.AddText(substrings[lexemeIndex].Value);
            }

            return result;
        }

        private bool SurroundedByDigits(List<Substring> substrings, int lexemeIndex)
        {
            return lexemeIndex + 1 < substrings.Count &&
                   specification.ProhibitionСharacters.Contains(substrings[lexemeIndex + 1].Value) ||
                   lexemeIndex - 1 >= 0 &&
                   specification.ProhibitionСharacters.Contains(substrings[lexemeIndex - 1].Value);
        }


        private void ResolveToken(Token token)
        {
            var parents = new List<Token>();
            ResolveTokenByParents(token, parents);
        }

        private void ResolveTokenByParents(Token token, List<Token> parents)
        {
            foreach (var parent in parents)
            {
                if (specification.ImpossibleNesting.ContainsKey(parent.TokenType.GetType()) &&
                    specification.ImpossibleNesting[parent.TokenType.GetType()].Contains(token.TokenType.GetType()))
                {
                    token.TokenType = new MdText();
                }
            }

            Token[] newParents = new Token[parents.Count + 1];
            parents.CopyTo(newParents);
            newParents[newParents.Length - 1] = token;
            foreach (var tkn in token.Tokens)
            {
                ResolveTokenByParents(tkn, newParents.ToList());
            }
        }

        private bool TryCloseToken(Delimiter closingDelimiter, out Token closedToken)
        {
            closedToken = null;
            var token = CurrentToken;
            if (token.Delimiter is null)
                return false;
            while (token != null && !DelimitersArePair(token.Delimiter, closingDelimiter))
            {
                if (token.ParentToken?.Delimiter is null)
                {
                    return false;
                }

                token = token.ParentToken;
            }

            token.Closed = true;
            token.TokenType = specification.MdToTokenTypes[token.Delimiter.Value];
            closedToken = token;
            return true;
        }

        private bool DelimitersArePair(Delimiter startingDelimiter, Delimiter closingDelimiter)
        {
            if (specification.MdStartingToClosingDelimiters.ContainsKey(startingDelimiter.Value))
            {
                return specification.MdStartingToClosingDelimiters[startingDelimiter.Value] == closingDelimiter.Value;
            }

            return false;
        }
    }
}