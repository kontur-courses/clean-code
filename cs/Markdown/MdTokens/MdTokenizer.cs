﻿using System;
using System.Collections.Generic;
 using System.Linq;
 using Markdown.Tokenizer;

namespace Markdown.MdTokens
{
    public class MdTokenizer : ITokenizer
    {
        private HashSet<string> SpecialSymbols;

        public MdTokenizer()
        {
            SpecialSymbols = new HashSet<string> {"_", "#", "__"};
        }
        public IEnumerable<IToken> MakeTokens(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (text == "") throw new ArgumentException("Text is empty");
            return text.Split().Select(str => MakeToken(str));
        }

        private MdToken MakeToken(string text)
        {
            var beginningSymbol = GetBeginningSpecialSymbol(text);
            var endingSymbol = GetEndingSpecialSymbol(text);
            if (text == beginningSymbol || text == endingSymbol)
                beginningSymbol = endingSymbol = "NONE";
            var content = GetContent(text, beginningSymbol, endingSymbol);
            return new MdToken(content, beginningSymbol, endingSymbol);
        }

        private string GetBeginningSpecialSymbol(string text)
        {
            var specialSymbolBeginning = text[0].ToString();
            var specialSymbolBeginningDoubles = "" + text[0] + text[1];
            return GetSpecialSymbol(specialSymbolBeginning, specialSymbolBeginningDoubles);
        }
        
        private string GetEndingSpecialSymbol(string text)
        {
            var specialSymbolEnding = text[text.Length - 1].ToString();
            var specialSymbolEndingDoubles = "" + text[text.Length - 1] + text[text.Length - 2];
            return GetSpecialSymbol(specialSymbolEnding, specialSymbolEndingDoubles);
        }
        
        private string GetContent(string text, string beginningSymbol, string endingSymbol)
        {
            var content = text;
            if (IsSymbolShielded(beginningSymbol))
                content = content.Substring(1);
            else if(beginningSymbol != "NONE")
                content = content.Substring(beginningSymbol.Length);
            if (IsSymbolShielded(endingSymbol))
                content = content.Remove(content.Length - 2, 1);
            else if(endingSymbol != "NONE")
                content = content.Remove(content.Length - endingSymbol.Length);
            return content;
        }

        private string GetSpecialSymbol(string singular, string doubled)
        {
            var specialSymbol = singular;
            if (!SpecialSymbols.Contains(singular))
                specialSymbol =  "NONE";
            if (IsSymbolShielded(doubled))
                specialSymbol = @"\";
            else if (SpecialSymbols.Contains(doubled))
                specialSymbol = doubled;
            
            return specialSymbol;
        }

        private bool IsSymbolShielded(string symbols)
        {
            return symbols.Contains(@"\");
        }
    }
}