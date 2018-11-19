using System;
using System.IO;

namespace Markdown
{
    public class Program
    {
        static void Main(string[] args)
        {
            var content = GetContent();
            var md = new Md();
            Console.WriteLine(md.Render(content));
        }

        public static string TryGetContent()
        {
            var path = Console.ReadLine();
            if (path == null)
                throw new NullReferenceException("Path to the file should not be null");
            return File.ReadAllText(path);
        }

        public static string GetContent()
        {
            var content = "";
            try
            {
                content = TryGetContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                content = GetContent();
            }
            return content;
        }
    }
}
