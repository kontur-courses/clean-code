using System;
using System.Collections;
using System.Text;

namespace Markdown.Conversion.Parsers
{
    public class LinkParser : IMarkParser
    {
        private LinkMark mark;
        private ParseHelper helper;
        private string linkText;
        private string link;
        private bool isFirst; 
        private int symbolsCount; 
        private string currentSymbol;
        private StringBuilder builder;
        
        public TokenMd GetToken(string text, int index, out int finalIndex)
        {
            link = default;
            linkText = default;
            finalIndex = index;
            builder = new StringBuilder();
            mark = new LinkMark();
            helper = new ParseHelper();
            isFirst = true;
            symbolsCount = 0;
            currentSymbol = "[";
            var isLink = true;

            while (finalIndex<text.Length)
            {
                if (isFirst && text[finalIndex] != '[')
                    isLink = false;
                 
                var resToken = HandleSquareBrackets(text, index, finalIndex, out finalIndex);
                if(resToken != null)
                    isLink = false;
                
                resToken = HandleOpenRoundBrackets(text, index, finalIndex, out finalIndex);
                if(resToken != null)
                    isLink = false;

                if (text[finalIndex] == ')' && currentSymbol == "(")
                {
                    link = builder.ToString();
                    finalIndex+= 2;
                    symbolsCount++;
                    mark.LinkText = linkText;
                    mark.Link = link;
                    
                    if(string.IsNullOrEmpty(linkText) && string.IsNullOrEmpty(link))
                        isLink = false;

                    if (symbolsCount == mark.AllSymbols.Length)
                    {
                        if(isLink)
                            return new TokenMd(linkText, mark);
                        var tokenText = text.Substring(index, finalIndex - index);
                        return new TokenMd(tokenText, new EmptyMark());
                    }
                }

                var resultText = text.Substring(index, finalIndex - index);
                return new TokenMd(resultText, new EmptyMark());
            }
            return new TokenMd(text.Substring(index, finalIndex - index), new EmptyMark());
        }

        private TokenMd ReturnFirstSymbolToken(int index, out int finalIndex)
        {
            finalIndex = index + 1;
            return new TokenMd("[", new EmptyMark());
        }

        private TokenMd HandleSquareBrackets(string text, int index, int finalIndex, out int newFinalIndex)
        {
            while (finalIndex < text.Length)
            {
                switch (text[finalIndex])
                {
                    case '[':
                        symbolsCount++;
                        isFirst = false;
                        finalIndex++;
                        continue;
                    case ']' when currentSymbol == "[":
                        linkText = builder.ToString();
                        builder = new StringBuilder();
                        currentSymbol = "]";
                        finalIndex++;
                        symbolsCount++;
                        continue;
                }

                if (currentSymbol == "[")
                {
                    builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);
                    continue;
                }

                newFinalIndex = finalIndex;
                return null;
            }
            newFinalIndex = finalIndex;
            return ReturnFirstSymbolToken(index, out finalIndex);
        }

        private TokenMd HandleOpenRoundBrackets(string text, int index, int finalIndex, out int newFinalIndex)
        {
            while (finalIndex < text.Length)
            {
                if (text[finalIndex] == '(' && currentSymbol == "]")
                {
                    currentSymbol = "(";
                    finalIndex++;
                    symbolsCount++;
                    continue;
                }

                if (text[finalIndex] == ')' && currentSymbol == "(")
                {
                    newFinalIndex = finalIndex;
                    return null;
                }
                
                if (currentSymbol == "(" && text[finalIndex] != '(')
                {
                    builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);
                    continue;
                }

                newFinalIndex = finalIndex;
                return null;
            }
            newFinalIndex = finalIndex;
            
            return ReturnFirstSymbolToken(index, out finalIndex);
        }
    }
}