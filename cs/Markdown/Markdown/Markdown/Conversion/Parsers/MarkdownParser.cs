using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownParser : IMarkdownParser
    {
        private List<(string DefiningSymbol,string LastSymbol)> markSymbols;
        private string text;
        private int index;

        public MarkdownParser(List<Mark> marks)
        {
            //получаем идентифицирующий символ и последний символ
            for (var i = 0; i < marks.Count; i++)
            {
                markSymbols.Add(GetSymbols(marks[i]));
            }
        }
        
        public List<TokenMd> GetTokens(string text)
        {
            
            this.text = text;
            var tokens = new List<TokenMd>();
            index = 0;
            var newIndex = 0;
            
            while (index < text.Length)
            {
                //примерный вид
                var token = GetToken(text, index, out newIndex);
                //tokens.Add(token);
                index = newIndex;
            }
            throw new NotImplementedException();
        }

        private TokenMd GetToken(string text, int index, out int finalIndex)
        {
            while (index<text.Length)
            {
                if (char.IsLetterOrDigit(text[index]))
                    GetSimpleWordToken();
                if (char.IsWhiteSpace(text[index]))
                        index++;
                //if(TryFindMark())
                //че то там
                
                    /*
                    foreach (var symbols  in markSymbols)
                    {
                        if (TryFindMark(out var symbol))
                        {
                            
                        }
                    }
                    */
            }
            throw new NotImplementedException(); 
        }

        private (string DefiningSymbol, string LastSymbol) GetSymbols(Mark mark)
        {
            var lastSymbol = mark.AllSymbols.Last();
            
            if (mark.AllSymbols.First() == mark.AllSymbols.Last())
                lastSymbol = Environment.NewLine;
            
            return (mark.DefiningSymbol, lastSymbol);
        }

        private bool TryFindMark(out (string DefiningSymbol, string LastSymbol) symbols)
        {
            foreach (var firstLastSymbols in markSymbols)
            {
                if (text[index].ToString() == firstLastSymbols.DefiningSymbol)
                {
                    symbols = firstLastSymbols;
                    return true;
                }

                if (index + firstLastSymbols.DefiningSymbol.Length < text.Length)
                {
                    for (var i = 0; i < firstLastSymbols.DefiningSymbol.Length; i++)
                    {
                        if (text[i] != firstLastSymbols.DefiningSymbol[i])
                        {
                            symbols = default;
                            return false;
                        }
                    }
                    symbols = firstLastSymbols;
                    return true;
                }
            }
            symbols = default;
            return false;
        }

        private TokenMd GetSimpleWordToken()
        {
            throw new NotImplementedException();
        }

        private bool TryFindEndMark(string lastSymbol, int index, out int finalIndex)
        {
            if(index + lastSymbol.Length < text.Length)
                    for (var i = 0; i < lastSymbol.Length; i++)
                    {
                        if (text[i] != lastSymbol[i])
                        {
                            finalIndex = index;
                            return false;
                        }
                    }

            finalIndex = index + 1;
            return true;
        }
    }
}    