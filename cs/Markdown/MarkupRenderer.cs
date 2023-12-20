﻿using Markdown.Converter;
using Markdown.Processor;
using Markdown.Syntax;
using Markdown.Token;

namespace Markdown;

public class MarkupRenderer
{
    private ISyntax syntax;
    private IParser parser;
    private IConverter converter;
    
    public MarkupRenderer(ISyntax syntax, IParser parser, IConverter converter)
    {
        this.syntax = syntax;
        this.parser = parser;
        this.converter = converter;
    }
    public string Render(string text)
    {
        var tagTokens = parser.ParseTokens(text);
        var converter = new MarkupConverter(syntax);
        return converter.ConvertTags(tagTokens, text);
    }
}