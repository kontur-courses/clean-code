using System;

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
                              "\nДля остановки введите \"stop\"\n");
            while (true)
            {
                var mdText = Console.ReadLine();
                if (mdText == "stop") break;
                Console.WriteLine(Md.Render(mdText));
            }
        }
    }
}