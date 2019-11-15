using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TagParsers
{
    public abstract class TagParser
    {
        public abstract string OpeningHtmlTag { get; }
        public abstract string ClosingHtmlTag { get; }
        public abstract string MdTag { get; }

        public string GetParsedLineFrom(string line)
        {
            if(line == null)
                throw new ArgumentException();
            var tokensToReplace = ParseLineOnMdTokens(line);
            var parsedLine = new StringBuilder();
            var indexNextToLastToken = 0;
            foreach (var token in tokensToReplace)
            {
                parsedLine.Append(line.Substring(indexNextToLastToken, token.StartIndex - indexNextToLastToken - MdTag.Length))
                    .Append(OpeningHtmlTag)
                    .Append(token.Value)
                    .Append(ClosingHtmlTag);
                indexNextToLastToken = GetIndexNextToToken(token);
            }
            if(indexNextToLastToken < line.Length)
                parsedLine.Append(line.Substring(indexNextToLastToken, line.Length - indexNextToLastToken));
            return parsedLine.ToString();
        }

        private IEnumerable<MdToken> ParseLineOnMdTokens(string line)
        {
            var tokens = new List<MdToken>();
            var i = 0;
            while (i + MdTag.Length < line.Length)
            {
                var substr = line.Substring(i, MdTag.Length);
                if (substr.Equals(MdTag) && ((i == 0 || NotContainEscapeSequence(line, i)) && !char.IsWhiteSpace(line[i + 1]))
                                         && TryBuildToken(line, i + MdTag.Length, out var token))
                {
                    if(IsNotSurroundedByDigits(line, token))
                        tokens.Add(token);
                    i = GetIndexNextToToken(token);
                }
                else
                    i++;
            }
            return tokens.Where(x => x.Length > 0);
        }

        private bool TryBuildToken(string line, int startIndex, out MdToken token)
        {
            for (var i = startIndex; i < line.Length - (MdTag.Length - 1); i++)
            {
                var substr = line.Substring(i, MdTag.Length);
                if (substr.Equals(MdTag) && !char.IsWhiteSpace(line[i - 1])
                                         && NotContainEscapeSequence(line, i))
                {
                    token = new MdToken(line.Substring(startIndex, i - startIndex),
                        startIndex,
                        i - startIndex);
                    return true;
                }
            }
            token = null;
            return false;
        }

        private bool NotContainEscapeSequence(string line, int currentIndex)
        {
            return currentIndex > 1 && line[currentIndex - 1] == '\\' && line[currentIndex - 2] == '\\' ||
                    currentIndex > 0 && line[currentIndex - 1] != '\\';
        }

        private bool IsNotSurroundedByDigits(string line, MdToken token)
        {
            return (token.StartIndex - MdTag.Length == 0 || !char.IsDigit(line[token.StartIndex - MdTag.Length - 1])) &&
                   (GetIndexNextToToken(token) == line.Length || !char.IsDigit(line[GetIndexNextToToken(token)]));
        }

        private int GetIndexNextToToken(MdToken token)
        {
            return token.StartIndex + token.Length + MdTag.Length;
        }
    }
}