﻿namespace Markdown.Lexer;

public interface ILexer
{
    TokenizeResult Tokenize(string line);
}