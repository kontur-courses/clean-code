using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Markdown.Parser;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} <markdown> <html-output>");
                return;
            }

            try
            {
                var text = File.ReadAllText(args[0]);
                var md = new Md();
                var html = md.Render(text);

                File.WriteAllText(args[1], html);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Cant open file");
            }
        }
    }
}