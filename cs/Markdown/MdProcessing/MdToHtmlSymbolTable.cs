using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Markdown.MdProcessing
{
    public class MdToHtmlSymbolTable
    {
        private readonly Dictionary<string, string> symbolToHtmlBeginning;
        private readonly Dictionary<string, string> symbolToHtmlEnding;

        public MdToHtmlSymbolTable()
        {
            symbolToHtmlBeginning = new Dictionary<string, string>();
            symbolToHtmlEnding = new Dictionary<string, string>();
        }
        
        public void AddSymbol(string symbol, string htmlBeginning, string htmlEnding)
        {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));
            symbolToHtmlBeginning[symbol] = htmlBeginning ?? throw new ArgumentNullException(nameof(htmlBeginning));
            symbolToHtmlEnding[symbol] = htmlEnding ?? throw new ArgumentNullException(nameof(htmlEnding));
        }

        public string GetHtmlBeginning(string symbol)
        {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));
            if(!symbolToHtmlBeginning.ContainsKey(symbol)) throw new ArgumentException("No symbol found");
            return symbolToHtmlBeginning[symbol];
        }

        public string GetHtmlEnding(string symbol)
        {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));
            if(!symbolToHtmlEnding.ContainsKey(symbol)) throw new ArgumentException("No symbol found");
            return symbolToHtmlEnding[symbol];
        }
    }
}