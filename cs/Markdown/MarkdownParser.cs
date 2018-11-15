using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Markdown
{
    public class MarkdownParser
    {
        private string markdownInput;
        public Token currentToken;
        private int index;


        public MarkdownParser(string markdownInput)
        {
            this.markdownInput = markdownInput;
            currentToken = new Token();
            var firstToken = new Token(new Delimiter("", 0, false, true));
            currentToken.AddToken(firstToken);
            currentToken = firstToken;
            index = 0;
        }


        public bool IsCorrectToken(Token token)
        {
            return true;
        }

        private bool TokensCanBeNested(Token childToken, Token parentToken)
        {
            return false;
        }

        public Token GetTokens()
        {
            var searcher = new StringSearcher();
            var substrings = searcher.SplitBySubstrings(Specification.Delimiters, markdownInput);
            substrings.Add(new Substring(markdownInput.Length, ""));

            foreach (var substring in substrings)
            {
                if (!Specification.Delimiters.Contains(substring.Value))
                {
                    currentToken.AddText(substring.Value);
                    if (currentToken.StartingDelimiter.delimiter == "\\")
                    {
                        currentToken.closed = true;
                        currentToken = currentToken.ParentToken;
                    }
                }
                else
                {
                    var canBeClosing = Specification.CanBeClosing(substring, markdownInput);
                    var canBeStarting = Specification.CanBeStarting(substring, markdownInput);
                    var delimiter = new Delimiter(substring.Value, substring.Index, canBeClosing, canBeStarting);
                    var closed = false;
                    if (delimiter.canBeClosing)
                    {
                        Token closedToken;
                        closed = TryCloseToken(delimiter, out closedToken);
                        if (closed)
                        {
                            ResolveToken(closedToken);
                            currentToken = closedToken.ParentToken;
                            if (closedToken.ClosingDelimiter.delimiter == " ")
                            {
                                var newTextToken = new Token(new Delimiter("", 0, false, true));
                                currentToken.AddToken(newTextToken);
                                currentToken = newTextToken;
                            }

                            continue;
                        }
                    }

                    if (!closed && delimiter.canBeStarting)
                    {
                        var newToken = new Token(delimiter);
                        currentToken.AddToken(newToken);
                        currentToken = newToken;
                    }
                    else
                    {
                        currentToken.AddText(delimiter.delimiter);
                    }
                }
            }

            return currentToken.RootToken;
        }

        private void ResolveToken(Token token)
        {
            var parents = new List<Token> {};
            ResolveTokenByParents(token, parents);
        }

        private void ResolveTokenByParents(Token token, List<Token> parents)
        {
            foreach (var parent in parents)
            {
                if (!Specification.possibleNesting[parent.tokenType].Contains(token.tokenType))
                {
                    token.tokenType = TokenType.text;
                }
            }

            Token[] newParents = new Token[parents.Count + 1];
            parents.CopyTo(newParents);
            newParents[newParents.Length - 1] = token;
            foreach (var tkn in token.tokens)
            {
                ResolveTokenByParents(tkn, newParents.ToList());
            }

        }


        private bool TryCloseToken(Delimiter closingDelimiter, out Token closedToken)
        {
            closedToken = null;
            var token = currentToken;
            if (token.StartingDelimiter is null)
                return false;
            while (!Specification.DelimetersCanBePair(token.StartingDelimiter, closingDelimiter))
            {
                if (token.ParentToken?.StartingDelimiter is null)
                {
                    return false;
                }

                token = token.ParentToken;
            }

            token.closed = true;

            token.ClosingDelimiter = closingDelimiter;


            token.tokenType = Specification.MdToTokenTypes[token.StartingDelimiter.delimiter];
            closedToken = token;
            return true;
        }


        
    }
}