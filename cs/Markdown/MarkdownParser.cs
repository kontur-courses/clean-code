using System;
using System.Collections.Generic;
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
        private HashSet<string> delimiters = new HashSet<string>() {"_", "__", "\\"};

        public MarkdownParser(string markdownInput)
        {
            this.markdownInput = markdownInput;
            currentToken = new Token();
            index = 0;
        }


        public bool IsCorrectToken(Token token)
        {
            return true;
        }


        public Token GetTokens()
        {
            var allDelimiters = GetAllDelimiters();
            var prevIndex = 0;
            var prevLength = 0;
            foreach (var delimiter in allDelimiters)
            {
                var textLength = delimiter.index - prevIndex - prevLength;
                currentToken.AddText(markdownInput.Substring(prevIndex + prevLength, textLength));


                if (currentToken.ParentToken?.StartingDelimiter != null &&
                    currentToken.ParentToken.StartingDelimiter.delimiter == "\\")
                {
                    if (textLength == 0)
                    {
                        currentToken.ParentToken.AddText(delimiter.delimiter);
                    }

                    currentToken = currentToken.ParentToken.ParentToken;
                }

                var closed = delimiter.canBeClosing && TryCloseToken(delimiter);

                if (!closed && delimiter.canBeStarting)
                {
                    var newTocken = new Token(delimiter);
                    currentToken.AddToken(newTocken);


                    currentToken = newTocken;
                }
                else if (!closed && !delimiter.canBeStarting)
                {
                    currentToken.AddText(delimiter.delimiter);
                }

                if (closed)
                {
                    currentToken = currentToken.ParentToken;
                }

                prevIndex = delimiter.index;
                prevLength = delimiter.delimiter.Length;
            }

            currentToken.AddText(markdownInput.Substring(prevIndex + prevLength));

            return currentToken.RootToken;
        }

        private bool TryCloseToken(Delimiter closingDelimiter)
        {
            var token = currentToken;
            while (token.StartingDelimiter.delimiter != closingDelimiter.delimiter &&
                   token.ClosingDelimiter is null)
            {
                if (token.ParentToken is null)
                    return false;
                token = token.ParentToken;
            }

            if (token.StartingDelimiter.delimiter == closingDelimiter.delimiter)
            {
                token.ClosingDelimiter = closingDelimiter;
            }

            if (IsCorrectToken(token))
                return true;
            else
            {
                token.ClosingDelimiter = null;
                token.StartingDelimiter = null;
                token.InsertText(0, closingDelimiter.delimiter);

                return false;
            }
        }


        private List<Delimiter> GetAllDelimiters()
        {
            var result = new List<Delimiter>();
            var searcher = new StringSearcher();
            var delimitersSubstrings = searcher.GetAllSubstrings(delimiters, markdownInput);

            foreach (var substring in delimitersSubstrings)
            {
                var canBeClosing = CanBeClosing(substring);
                var canBeStarting = CanBeStarting(substring);
                var escaped = IsEscaped(substring);
                if ((canBeStarting || canBeClosing) && !escaped)
                {
                    var delimiter = new Delimiter(substring.Value, substring.Index, canBeClosing, canBeStarting);
                    result.Add(delimiter);
                }
            }

            return result;
        }

        private bool IsEscaped(Substring substring)
        {
            return substring.Index != 0 && markdownInput[substring.Index - 1] == '\\';
        }

        private bool CanBeStarting(Substring substring)
        {
            return substring.Index + substring.Length + 1 < markdownInput.Length &&
                   markdownInput[substring.Index + substring.Length + 1] != ' ';
        }

        private bool CanBeClosing(Substring substring)
        {
            return substring.Index != 0 && markdownInput[substring.Index - 1] != ' ';
        }
    }

    [TestFixture]
    public class MarkdownParser_Tests
    {
    }
}