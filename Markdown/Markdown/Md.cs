using System;

namespace Markdown
{
    public static class Md
    {   
        public static string Render(string source)
        {
            var delimeters = new (string delimeter, TokenType tokenType)[]
            {
                ValueTuple.Create(BoldStyleElement.DoubleKeyword, BoldStyleElement.TokenType),
                ValueTuple.Create(ItalicStyleElement.DoubleKeyword, ItalicStyleElement.TokenType),
                ValueTuple.Create(@"\", TokenType.EscapingDelimiter),
                ValueTuple.Create(@"[", TokenType.OpeningLinkHrefDelimiter),
                ValueTuple.Create(@"]", TokenType.ClosingLinkNameDelimiter),
                ValueTuple.Create(@"(", TokenType.OpeningLinkHrefDelimiter),
                ValueTuple.Create(@")", TokenType.ClosingLinkHrefDelimiter),
            };
            
            var parser = new MarkdownParser(source);          
            var interpreter = new MarkdownInterpreter(parser.Parse());
            return interpreter.GetHtmlCode();
       }
    }
}
