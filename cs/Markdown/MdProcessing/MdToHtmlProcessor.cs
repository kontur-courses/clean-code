using System;
using System.Collections.Generic;
using Markdown.MdTokens;

namespace Markdown.MdProcessing
{
    public class MdToHtmlProcessor : IMdProcessor
    {
        private readonly MdToHtmlSymbolTable htmlSymbolTable;

        public MdToHtmlProcessor()
        {
            htmlSymbolTable = new MdToHtmlSymbolTable();
            htmlSymbolTable.AddSymbol("_", "<em>", "</em>");
            htmlSymbolTable.AddSymbol("__", "<strong>", "</strong>");
            htmlSymbolTable.AddSymbol("NONE", "", "");
            htmlSymbolTable.AddSymbol(@"\", "", "");
        }
        
        public string GetProcessedResult(MdToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (token.Content == "") throw new ArgumentException("Token content is empty");
            
            var htmlBeginning = htmlSymbolTable.GetHtmlBeginning(token.BeginningSpecialSymbol);
            var htmlEnding = htmlSymbolTable.GetHtmlEnding(token.EndingSpecialSymbol);
            return htmlBeginning + token.Content + htmlEnding;
        }
    }
}