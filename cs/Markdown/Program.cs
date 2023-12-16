// For manual testing.

using Markdown;
using Markdown.Tags;

var processor = new MD();

processor.AddFactoryFor("__", () => new BoldTag());
processor.AddFactoryFor("_", () => new ItalicTag());
processor.AddFactoryFor("# ", () => new HeaderTag());
processor.AddFactoryFor("\\n", () => new NewlineTag());

var textExample = @"# _Billie Jean_ is not my __lover__ 
                   She's just a __girl__ who __claims _that_ I am__ the one 
                   But the __kid is not my son__ 
                   She __says _I am__ the_ one, but the _kid is __not__ my son_";

var tokens = processor.Render(textExample);

Console.WriteLine();
