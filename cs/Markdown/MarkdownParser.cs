using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownParser
    {
        private readonly string markdownInput;
        public Token CurrentToken;


        public MarkdownParser(string markdownInput)
        {
            this.markdownInput = markdownInput;
            CurrentToken = new Token();
        }


        public Token GetTokens()
        {
            var searcher = new StringSearcher();
            var allKeyWords = new HashSet<string>();
            allKeyWords.UnionWith(Specification.KeyWords);
            allKeyWords.UnionWith(Specification.Digits);
            var substrings = searcher.SplitBySubstrings(allKeyWords, markdownInput);

            for (int i = 0; i < substrings.Count; i++)
            {
                if (substrings[i].Value == "\\")
                {
                    if (i + 1 < substrings.Count && Specification.KeyWords.Contains(substrings[i + 1].Value))
                    {
                        CurrentToken.AddText(substrings[i + 1].Value);
                        i += 1;
                    }
                    else
                    {
                        CurrentToken.AddText(substrings[i].Value);
                    }

                    continue;
                }

                if (Specification.KeyWords.Contains(substrings[i].Value))
                {
                    if (i + 1 < substrings.Count && Specification.Digits.Contains(substrings[i + 1].Value) ||
                        i - 1 >= 0 && Specification.Digits.Contains(substrings[i - 1].Value))
                    {
                        CurrentToken.AddText(substrings[i].Value);
                        continue;
                    }
                }

                var substring = substrings[i];
                if (!Specification.KeyWords.Contains(substring.Value))
                {
                    CurrentToken.AddText(substring.Value);
                }

                else
                {
                    var canBeClosing = Specification.CanBeClosing(substring, markdownInput);
                    var canBeStarting = Specification.CanBeStarting(substring, markdownInput);
                    var delimiter = new Delimiter(substring.Value, substring.Index, canBeClosing, canBeStarting);
                    if (delimiter.CanBeClosing)
                    {
                        var closed = TryCloseToken(delimiter, out var closedToken);
                        if (closed)
                        {
                            ResolveToken(closedToken);
                            CurrentToken = closedToken.ParentToken;
                            continue;
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
            }

            return CurrentToken.RootToken;
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
                if (Specification.ImpossibleNesting.ContainsKey(parent.TokenType) &&
                    Specification.ImpossibleNesting[parent.TokenType].Contains(token.TokenType))
                {
                    token.TokenType = TokenType.Text;
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
            if (token.StartingDelimiter is null)
                return false;
            while (token != null && !Specification.DelimitersCanBePair(token.StartingDelimiter, closingDelimiter))
            {
                if (token.ParentToken?.StartingDelimiter is null)
                {
                    return false;
                }

                token = token.ParentToken;
            }

            token.Closed = true;

            token.ClosingDelimiter = closingDelimiter;


            token.TokenType = Specification.MdToTokenTypes[token.StartingDelimiter.Value];
            closedToken = token;
            return true;
        }
    }
}