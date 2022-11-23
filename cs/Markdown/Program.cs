using System;
using System.Text;

namespace Markdown
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Введите текст в формате markdown.\n" +
                              "Поддерживаются теги:\n" +
                              "   _text_ - курсив\n" +
                              "   __text__ - жирный\n" +
                              "   # text - заголовок\n" +
                              "   [text](link)\n" +
                              "\nДля остановки и получения результата введите \"stop\"\n");
            
            var sb = new StringBuilder();
            while (true)
            {
                var mdText = Console.ReadLine();
                if (mdText == "stop") break;
                sb.Append(mdText);
                sb.Append('\n');
            }
            
            Console.WriteLine(Md.Render(sb.ToString()));
        }
    }
}