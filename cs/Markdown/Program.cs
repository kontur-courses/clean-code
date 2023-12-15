// For manual testing.

using Markdown;

var text = @"# Hello!\n My \_name\_ is __Grigory__";
var tokens = Tokenizer.CollectTokens(text);

Console.WriteLine();