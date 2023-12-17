using Markdown;
using Markdown.Tags;

Console.WriteLine(new Md(Tag.EmTag, Tag.HeaderTag, Tag.HeaderTag).Render("_a_"));