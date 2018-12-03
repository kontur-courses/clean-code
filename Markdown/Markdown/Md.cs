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
            };
            
            var lexer = new MarkdownLexer(source, delimeters);
            var parser = new MarkdownParser(lexer);          
            var interpreter = new MarkdownInterpreter(parser.Parse());
            return interpreter.GetHtmlCode();
       }
    }
}
