using Markdown;
using Markdown.Tags;

Console.WriteLine(new Md(new HeaderTag())
    .Render("# Заголовок __с _разными_ символами__"));