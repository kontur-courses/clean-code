using System;
using System.Text;
using Markdown.Parsing;
using Markdown.Tokenizing;

namespace Markdown;

public class Md
{
    private readonly Lexer lexer;
    private readonly Parser parser;
    private readonly HtmlRender htmlRender;

    public Md(Lexer lexer, Parser parser, HtmlRender htmlRender)
    {
        this.lexer = lexer;
        this.parser = parser;
        this.htmlRender = htmlRender;
    }

    public string Render(string input)
    {
        var tokens = lexer.Tokenize(input);
        var document = parser.Parse(tokens);
        return htmlRender.Render(document);
    }
}