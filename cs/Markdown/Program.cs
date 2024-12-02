using Markdown;
using Markdown.Converters;
using Markdown.Tokenizers;

var tokenizer = new MdTokenizer();
var converter = new HtmlConverter();
var markdown = new Md(tokenizer, converter);

const string firstExample = "This _is_ a __sample__ markdown _file_.";
const string secondExample = "#This is another __sample__ markdown _file_";

const string input = firstExample;

var result = markdown.Render(input);

Console.WriteLine(result);