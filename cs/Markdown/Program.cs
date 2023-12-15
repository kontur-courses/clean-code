// For manual testing.

using Markdown.Tokens;

var text = @"__ Hello, my _name_ is __Grigory__. \nI am 1_9_ y.o.__ __";
var tokens = Tokenizer.CollectTokens(text);

Console.WriteLine();