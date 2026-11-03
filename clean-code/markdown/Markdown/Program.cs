using Markdown;

string input = "# Заголовок с разными символами __ и _курсивом___";
string output = new Md().Render(input);
Console.WriteLine(output);