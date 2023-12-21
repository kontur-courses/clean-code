using Markdown;

string filePath = "Templates/SampleText.txt";

try
{
    string text = File.ReadAllText(filePath);
    string renderedText = MdProcessor.Render(text);

    Console.WriteLine(renderedText);
}
catch (IOException e)
{
    Console.WriteLine("Ошибка чтения файла: " + e.Message);
}