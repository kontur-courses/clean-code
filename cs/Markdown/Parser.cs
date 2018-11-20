using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Parser
    {
        private readonly TokenReader reader;
        private readonly ITokenType[] allTokenTypes;
        private readonly Stack<ITokenType> typesNesting;
        private bool currentUnderscoreIsClosing;
        private bool previousUnderscoreIsClosing;
        private ITokenType currentUnderscoreType;
        private readonly int leftLength;
        public readonly string MdText;

        public Parser(string mdText, ITokenType[] tokenTypes)
        {
            typesNesting = new Stack<ITokenType>();
            reader = new TokenReader(mdText);
            MdText = mdText;
            allTokenTypes = tokenTypes.ToArray();
            leftLength = tokenTypes.Max(tokenType => tokenType.GetMarker().Length);
        }

        public Token[] GetTokens()
        {
            var tokens = new List<Token>();

            while (reader.Position < MdText.Length - 1)
            {
                currentUnderscoreType = null;
                var previousType = typesNesting.Any() ? typesNesting.Peek() : null;
                var openTypes = previousType == null ? allTokenTypes : previousType.SupportedInnerTypes();
                var token = reader.ReadUntil(ch => CheckIfOpen(ch, openTypes) || CheckIfClosing(ch, typesNesting));
                token.Type = previousType;

                if (!previousUnderscoreIsClosing)
                    token.Opened = true;

                if (currentUnderscoreIsClosing)
                    token.Closed = true;

                if (!currentUnderscoreIsClosing)
                {
                    if (currentUnderscoreType != null)
                        typesNesting.Push(currentUnderscoreType);
                    tokens.Add(token);
                }
                else
                {
                    if (currentUnderscoreType.GetType() != typesNesting.Peek().GetType())
                    {
                        tokens[tokens.Count - 1].Concat(token);
                        while (typesNesting.Peek().GetType() != currentUnderscoreType.GetType())
                            typesNesting.Pop();
                    }
                    else
                    {
                        tokens.Add(token);
                    }
                    typesNesting.Pop();
                }

                previousUnderscoreIsClosing = currentUnderscoreIsClosing;
                currentUnderscoreIsClosing = false;

                if (currentUnderscoreType != null) 
                    reader.Skip(currentUnderscoreType.GetMarker().Length);
            }

            if (!typesNesting.Any() || typesNesting.Peek() == null)
                return tokens.ToArray();
            while (typesNesting.Count != 0)
            {
                var indexOfLastOpenedAndNotClosed = GetIndexOfLastOpenedAndNotClosed(tokens.Where(token => token != null).ToArray(), typesNesting.Pop());
                tokens[indexOfLastOpenedAndNotClosed - 1].Concat(tokens[indexOfLastOpenedAndNotClosed]);
                tokens[indexOfLastOpenedAndNotClosed] = null;
            }


            return tokens.ToArray();
        }

        private int GetIndexOfLastOpenedAndNotClosed(Token[] tokens, ITokenType tokenType)
        {
            for (var i = tokens.Length - 1; i >= 0; i--)
            {
                if (tokens[i].Type.GetType() == tokenType.GetType())
                    return i;
            }

            return -1;
        }

        private bool CheckIfOpen(char ch, IEnumerable<ITokenType> possibleTypes)
        {
            var left = reader.Position == 0 ? ' ' : MdText[reader.Position - 1];
            var right = GetRightSide(reader.Position + 1);

            foreach (var tokenType in possibleTypes)
            {
                if (!tokenType.CheckIfOpen(ch, left, right))
                    continue;
                currentUnderscoreIsClosing = false;
                currentUnderscoreType = tokenType;
                return true;
            }

            return false;
        }

        private bool CheckIfClosing(char ch, IEnumerable<ITokenType> possibleTypes)
        {
            var left = reader.Position == 0 ? ' ' : MdText[reader.Position - 1];
            var right = GetRightSide(reader.Position + 1);

            foreach (var tokenType in possibleTypes)
            {
                if (!tokenType.CheckIfClosing(ch, left, right))
                    continue;
                currentUnderscoreIsClosing = true;
                currentUnderscoreType = tokenType;
                return true;
            }

            return false;
        }

        private string GetRightSide(int position)
        {
            var builder = new StringBuilder();
            var leftSide = MdText.Substring(position, Math.Min(MdText.Length - position, leftLength));
            builder.Append(leftSide);
            if (builder.Length < leftLength)
                builder.Append(' ', leftLength - builder.Length);
            return builder.ToString();
        }
    }
}