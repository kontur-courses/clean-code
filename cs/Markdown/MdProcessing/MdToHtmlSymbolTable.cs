using System.Collections.Generic;
using System.IO;

namespace Markdown.MdProcessing
{
    public class MdToHtmlSymbolTable
    {
        private Dictionary<string, string> symbolToHtmlBeginning;
        private Dictionary<string, string> symbolToHtmlEnding;

        public MdToHtmlSymbolTable()
        {
            symbolToHtmlBeginning = new Dictionary<string, string>();
            symbolToHtmlEnding = new Dictionary<string, string>();
        }
        
        public void AddSymbol(string symbol, string htmlBeginning, string htmlEnding)
        {
            symbolToHtmlBeginning[symbol] = htmlBeginning;
            symbolToHtmlEnding[symbol] = htmlEnding;
        }

        public string GetHtmlBeginning(string symbol)
        {
            return symbolToHtmlBeginning[symbol];
        }

        public string GetHtmlEnding(string symbol)
        {
            return symbolToHtmlEnding[symbol];
        }
    }
}