using System;
using Markdown.ASTNodes.StyleElement;

namespace Markdown
{
    public static class Md
    {   
        public static string Render(string source)
        {
            var delimeters = new (string delimeter, TokenType tokenType)[]
            {
                (BoldStyleElement.DoubleKeyword, BoldStyleElement.TokenType),
                (ItalicStyleElement.DoubleKeyword, ItalicStyleElement.TokenType),
                (@"\", TokenType.EscapingDelimiter),
            };
            
            var lexer = new MarkdownLexer(source, delimeters);
            var parser = new MarkdownParser(lexer);          
            var interpreter = new MarkdownInterpreter(parser.Parse());
            return interpreter.Interpret();
       }
    }
}
