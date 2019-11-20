using System;
using System.IO;
using Markdown.Parser;

namespace Markdown.MarkDown
{
    internal class MarkDown
    {
        public static void Main()
        {
            Console.WriteLine("путь до файла с MD-разметкой");
            var pathToMdFile = Console.ReadLine();
            var mdText = new StreamReader(pathToMdFile ?? throw new InvalidOperationException()).ReadToEnd();
            var processor = new MdProcessor(new MdTagParser());
            var htmlText = processor.Render(mdText);
            Console.WriteLine("где сохранить файл с HTML-разметкой (включая название файла)");
            var pathToSave = Console.ReadLine();
            var fileToSave = new StreamWriter(pathToSave ?? throw new InvalidOperationException());
            fileToSave.Write(htmlText);
            fileToSave.Close();
        }
    }
}
