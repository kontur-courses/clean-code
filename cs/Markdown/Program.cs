// For manual testing.

using Markdown.Tokens;

var text = @"\_43\_5 \\\\ ук рф__ _ста__т_ья__ за у_б_ийс_тво_";
var tokens = Tokenizer.CollectTokens(text);

Console.WriteLine();
