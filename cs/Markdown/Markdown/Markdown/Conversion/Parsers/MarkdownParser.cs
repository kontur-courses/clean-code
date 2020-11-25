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
        private string textMd;

        private Dictionary<Mark, IMarkParser> marks;

        public MarkdownParser(string text, Dictionary<Mark, IMarkParser> marksInfo=null)
        {
            textMd = text;
            marks = new Dictionary<Mark, IMarkParser>
            {
                {new HeadMark(), new HeadParser()},
                {new StrongMark(), new StrongParser()},
                {new ItalicMark(), new ItalicParser()},
                {new LinkMark(), new LinkParser()}
            };
            if(marksInfo!=null)
            {
                foreach (var markInfo in marksInfo)
                    marks.Add(markInfo.Key, markInfo.Value);
            }
        }
        
        public List<TokenMd> GetTokens(string text)
        {
            textMd = text;
            var tokens = new List<TokenMd>();
            var index = 0;
            var finalIndex = 0;
            
            while (index < text.Length)
            {
                var token = GetToken(text, index, out finalIndex);
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

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Mark != null && tokens[i].Mark.GetType() == mark.GetType() &&
                    tokens[i].Token.Contains(symbol))
                {
                    tokenIndex = i;
                    isMark = true;
                    continue;
                }
                if (tokens[i].Mark != null && tokens[i].Mark.GetType() == mark.GetType() && isMark)
                    isMark = false;
                if (tokens[i].Mark == null && tokens[i].Token.Contains(symbol) && isMark)
                {
                    tokens[tokenIndex] = new TokenMd(tokens[tokenIndex].Token, null);
                    tokens[tokenIndex].InnerTokens = null;
                }
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
                return new TokenMd(" ", null);
            }

            if (text[index] == '\\')
            {
                Escaping(text, index, out finalIndex);
                index = finalIndex;
            }

            if (TryFindMarkToken(text, index, out finalIndex, out var token)
                && (IsEndSpaceOrNewLine(text, finalIndex) || token.Mark == null))
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
                if (mark.Key.GetType() == typeof(ItalicMark) ) 
                    if(index+1<text.Length 
                       && text[index+1]!='_' 
                       && text[index].ToString() == mark.Key.DefiningSymbol)
                    {
                        symbols = mark.Key;
                        return true;
                    }
                if(mark.Key.GetType() != typeof(ItalicMark) && text[index].ToString() == mark.Key.DefiningSymbol)
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
            if(index + symbols.Length < text.Length)
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
            Mark mark = null;
           
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

                if (mark != null && helper.IsSymbols(finalIndex, text, mark.AllSymbols.Last()) && isMarkToken)
                {
                    builder = new StringBuilder();
                    innerTokens.Add(marks[mark].GetToken(text, startTokenIndex, out finalIndex));
                }

                if (mark!= null && helper.IsSymbols(finalIndex, text, mark.DefiningSymbol) && !isMarkToken)
                {
                    innerTokens.Add(new TokenMd(builder.ToString(), null));
                    builder =new StringBuilder();
                    startTokenIndex = finalIndex;
                    isMarkToken = true;
                }
   
                if(finalIndex==text.Length ) 
                    continue;
                
                builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);
            }
            if (innerTokens.Count>0)
                return GetTokenWithInners(innerTokens, builder);
            
            return new TokenMd(builder.ToString(), null);
        }

        private bool IsUnderlineWithDigits(string text, int finalIndex)
        {
            return (finalIndex!=0 && Char.IsDigit(text[finalIndex - 1])) || (finalIndex+1<text.Length && Char.IsDigit(text[finalIndex + 1]));
        }
        
        private List<TokenMd> GetAllInnerTokens(List<TokenMd> tokens)
        {
            var resultTokens = tokens;

            for (int i = 0; i < resultTokens.Count; i++)
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
                    
                    if (token.Mark!=null 
                        && parsedToken.Mark!=null 
                        && token.Mark.GetType() == typeof(ItalicMark) 
                        && parsedToken.Mark.GetType() == typeof(StrongMark))
                        return new TokenMd(resultToken.Token, null);

                    parsedToken.External = token;
                    index = finalIndex;
                    if (parsedToken.Mark == null && parsedToken.InnerTokens!=null && parsedToken.InnerTokens.Count == 0)
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
            innerTokens.Add(new TokenMd(builder.ToString(), null));
            for (int i = 0; i < innerTokens.Count; i++)
            {
                if (innerTokens[i].Mark != null)
                    markTokenCount++;
                innerBuilder.Append(innerTokens[i].Token);
            }
                
            var resultToken = new TokenMd(innerBuilder.ToString(), null);
            for (int i = 0; i < innerTokens.Count; i++)
                resultToken.InnerTokens
                    .Add(new TokenMd(innerTokens[i].Token, innerTokens[i].Mark, resultToken));

            if (markTokenCount == 0)
                resultToken.InnerTokens = null;
            return resultToken;
        }
    }
}    