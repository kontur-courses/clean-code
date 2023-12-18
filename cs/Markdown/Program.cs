using Markdown;
using Markdown.Tags;

Console.WriteLine(new Md(new EmTag(), new HeaderTag(), new HeaderTag()).Render("_a_"));