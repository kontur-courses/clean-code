using Markdown;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;

var md = new Md(new Lexer(), new Parser(), new Renderer());
var input = Console.In.ReadToEnd();
var html = md.Render(input);
Console.Write(html);