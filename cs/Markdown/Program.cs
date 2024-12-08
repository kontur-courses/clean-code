using Markdown;

var md = new MarkDown();
var result = md.Render("__bold _italic_ text__");
Console.WriteLine(result);