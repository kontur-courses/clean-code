using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Conversion.Parsers;
using Markdown.Parsers;

namespace Markdown
{
    public class MarkdownParser : IMarkdownParser
    {
        private Dictionary<Mark, IMarkParser> marks;

        public MarkdownParser(Dictionary<Mark, IMarkParser> marksInfo=null)
        {
            marks = new Dictionary<Mark, IMarkParser>
            {
                {new HeadMark(), new HeadParser()},
                {new StrongMark(), new StrongParser()},
                {new ItalicMark(), new ItalicParser()},
                {new LinkMark(), new LinkParser()}
            };
            if (marksInfo == null) return;
            foreach (var (mark, parser) in marksInfo)
                marks.Add(mark, parser);
        }
        
        public List<TokenMd> GetTokens(string text)
        {
            var tokens = new List<TokenMd>();
            var index = 0;

            while (index < text.Length)
            {
                var token = GetToken(text, index, out var finalIndex);
                tokens.Add(token);
                index = finalIndex;
            }
            
            HandleItalicStrongIntersect(tokens);

            return GetAllInnerTokens(tokens);
        }

        private void HandleIntersect(List<TokenMd> tokens, Mark mark, string symbol)
        {
            var tokenIndex = 0;
            var isMark = false;

            for (var i = 0; i < tokens.Count; i++)
            {
                if (!(tokens[i].Mark is EmptyMark) && tokens[i].Mark.GetType() == mark.GetType() &&
                    tokens[i].Token.Contains(symbol))
                {
                    tokenIndex = i;
                    isMark = true;
                    continue;
                }
                if (!(tokens[i].Mark is EmptyMark) && tokens[i].Mark.GetType() == mark.GetType() && isMark)
                    isMark = false;
                
                if (tokens[i].Mark is EmptyMark && tokens[i].Token.Contains(symbol) && isMark)
                    tokens[tokenIndex] = new TokenMd(tokens[tokenIndex].Token, new EmptyMark()) {InnerTokens = null};
            }
        }
        
        private void HandleItalicStrongIntersect(List<TokenMd> tokens)
        {
            HandleIntersect(tokens, new ItalicMark(), "__");
            HandleIntersect(tokens, new StrongMark(), "_");
        }
        
        
        private TokenMd GetToken(string text, int index, out int finalIndex)
        {
            if (char.IsWhiteSpace(text[index]))
            {
                index++;
                finalIndex = index;
                return new TokenMd(" ", new EmptyMark());
            }

            if (text[index] == '\\')
            {
                Escaping(text, index, out finalIndex);
                index = finalIndex;
            }

            if (TryFindMarkToken(text, index, out finalIndex, out var token)
                && (IsEndSpaceOrNewLine(text, finalIndex) || token.Mark == new EmptyMark()))
                return token;

            return GetWordToken(text, index, out finalIndex);
        }
        
        private void Escaping(string text, int index, out int finalIndex)
        {
            finalIndex = index;
            
            if (TryFindMark(text, finalIndex+1, out var symbol))
                finalIndex += symbol.DefiningSymbol.Length+1;
            
            if (text[finalIndex+1] == '\\')
                finalIndex+=2;
        }

        private bool TryFindMarkToken(string text, int index,out int finalIndex, out TokenMd token)
        {
            if (TryFindMark(text, index, out var symbol))
            {
                token= marks[symbol].GetToken(text, index, out finalIndex);
                return true;
            }
            finalIndex = index;
            
            token = default;
            return false;
        }
        
        private bool TryFindMark(string text, int index, out Mark symbols)
        {
            foreach (var mark in marks)
            {
                if (mark.Key is ItalicMark) 
                    if(index+1<text.Length 
                       && text[index+1]!='_' 
                       && text[index].ToString() == mark.Key.DefiningSymbol)
                    {
                        symbols = mark.Key;
                        return true;
                    }
                if(mark.Key is ItalicMark && text[index].ToString() == mark.Key.DefiningSymbol)
                {
                    symbols = mark.Key;
                    return true;
                }
                if(IsSymbols(index, text, mark.Key.DefiningSymbol))
                {
                    symbols = mark.Key;
                    return true;
                }
            }

            symbols = default;
            return false;
        }
        
        private bool IsEndSpaceOrNewLine(string text, int index)
        {
            if (index + 1 > text.Length)
                return true;

            if (index < text.Length && char.IsWhiteSpace(text[index]))
                return true;

            return false;
        }
        
        private bool IsSymbols(int index, string text, string symbols)
        {
            var count = 0;
            if (index + symbols.Length >= text.Length) 
                return false;
            for (var i = 0; i < symbols.Length; i++)
            {
                if (text[i+index] == symbols[i])
                    count++;

                if (count == symbols.Length)
                    return true;
            }

            return false;
        }

        private TokenMd GetWordToken(string text, int index, out int finalIndex)
        {
            var startTokenIndex = 0;
            Mark mark = new EmptyMark();
           
            var isMarkToken = false;
            var helper = new ParseHelper();
            var innerTokens = new List<TokenMd>();
            finalIndex = index;
            var builder = new StringBuilder();

            while (finalIndex<text.Length && !char.IsWhiteSpace(text[finalIndex]))
            {
                if (text[finalIndex] == '_' && IsUnderlineWithDigits(text, finalIndex))
                    builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);

                if (TryFindMark(text, finalIndex, out var newMark) && newMark!=mark)
                    mark = newMark;

                if (!(mark is EmptyMark) && helper.IsSymbols(finalIndex, text, mark.AllSymbols.Last()) && isMarkToken)
                {
                    builder = new StringBuilder();
                    innerTokens.Add(marks[mark].GetToken(text, startTokenIndex, out finalIndex));
                }

                if (!(mark is EmptyMark) && helper.IsSymbols(finalIndex, text, mark.DefiningSymbol) && !isMarkToken)
                {
                    innerTokens.Add(new TokenMd(builder.ToString(), new EmptyMark()));
                    builder = new StringBuilder();
                    startTokenIndex = finalIndex;
                    isMarkToken = true;
                }
   
                if(finalIndex==text.Length ) 
                    continue;
                
                builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);
            }
            return innerTokens.Count>0 
                ? GetTokenWithInners(innerTokens, builder) 
                : new TokenMd(builder.ToString(), new EmptyMark());
        }

        private bool IsUnderlineWithDigits(string text, int finalIndex)
        {
            return (finalIndex!=0 && Char.IsDigit(text[finalIndex - 1])) 
                   || (finalIndex+1<text.Length && Char.IsDigit(text[finalIndex + 1]));
        }
        
        private List<TokenMd> GetAllInnerTokens(List<TokenMd> tokens)
        {
            var resultTokens = tokens;

            for (var i = 0; i < resultTokens.Count; i++)
                resultTokens[i] = GetInnerTokens(resultTokens[i]);

            return resultTokens;
        }
        
        private TokenMd GetInnerTokens(TokenMd token)
        {
            var innerTokens = new List<TokenMd>();
            var resultToken = token;
            var index = 0;
            
            if (resultToken.InnerTokens == null)
                return resultToken;

            if (resultToken.InnerTokens.Count == 0)
            {
                while (index<token.TokenWithoutMark.Length)
                {
                    var parsedToken = GetToken(resultToken.TokenWithoutMark, index, out var finalIndex);

                    if (!(token.Mark is EmptyMark)
                        && !(parsedToken.Mark is EmptyMark)
                        && token.Mark is ItalicMark
                        && parsedToken.Mark is StrongMark)
                        return new TokenMd(resultToken.Token, new EmptyMark());

                    parsedToken.External = token;
                    index = finalIndex;
                    if (parsedToken.Mark is EmptyMark && parsedToken.InnerTokens!=null && parsedToken.InnerTokens.Count == 0)
                        parsedToken.InnerTokens = null;
                    else
                        parsedToken = GetInnerTokens(parsedToken);

                    innerTokens.Add(parsedToken);
                }
                resultToken.InnerTokens = innerTokens;
            }
            else
                resultToken.InnerTokens = GetAllInnerTokens(resultToken.InnerTokens);
            
            
            return resultToken;
        }

        private TokenMd GetTokenWithInners(List<TokenMd> innerTokens, StringBuilder builder)
        {
            var markTokenCount = 0;
            var innerBuilder = new StringBuilder();
            innerTokens.Add(new TokenMd(builder.ToString(), new EmptyMark()));
            foreach (var innerToken in innerTokens)
            {
                if (!(innerToken.Mark is EmptyMark))
                    markTokenCount++;
                innerBuilder.Append(innerToken.Token);
            }
                
            var resultToken = new TokenMd(innerBuilder.ToString(), new EmptyMark());
            foreach (var innerToken in innerTokens)
                resultToken.InnerTokens
                    .Add(new TokenMd(innerToken.Token, innerToken.Mark, resultToken));

            if (markTokenCount == 0)
                resultToken.InnerTokens = null;
            return resultToken;
        }
    }
}    