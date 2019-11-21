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
            htmlSymbolTable.AddSymbol("#", "<h1>", "</h1>");
            htmlSymbolTable.AddSymbol("##", "<h2>", "</h2>");
            htmlSymbolTable.AddSymbol("###", "<h3>", "</h3>");
            htmlSymbolTable.AddSymbol("####", "<h4>", "</h4>");
            htmlSymbolTable.AddSymbol("#####", "<h5>", "</h5>");
            htmlSymbolTable.AddSymbol("######", "<h6>", "</h6>");
            htmlSymbolTable.AddSymbol("NONE", "", "");
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