using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Markdown.TokenConverters;

namespace Markdown.ConsoleUtils
{
    public static class ConsoleAssistant
    {
        private static void WriteConsoleHeader()
        {
            Console.WriteLine("########[Console Converter]########\n");
        }

        public static void ClearConsole()
        {
            Console.Clear();
            WriteConsoleHeader();
        }

        private static Dictionary<string, Type> GetAllConverterTypes()
        {
            return Assembly
                .GetAssembly(typeof(TokenConverter))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TokenConverter)))
                .ToDictionary(x => x.Name.ToLower());
        }


        public static TokenConverter SetupConverter()
        {
            ClearConsole();
            string key = null;
            var converterTypes = GetAllConverterTypes();

            while (true)
            {
                Console.WriteLine("Enter final convert type of file:");
                key = Console.ReadLine();

                if(key != null && 
                   !string.IsNullOrEmpty(converterTypes.Keys.FirstOrDefault(x => x.StartsWith(key))))
                    break;
                    
                ClearConsole();
                Console.WriteLine("***That type doesn't included in types library.***\n");
            }

            ClearConsole();
            Console.WriteLine("***Type successful applied!***\n");

            var type = converterTypes.Keys.First(x => x.StartsWith(key));
            return (TokenConverter)Activator.CreateInstance(converterTypes[type]);
        }

        public static string ReadTextFromFile(string endFileType)
        {
            ClearConsole();
            var isPathNotValid = true;
            string readText = null;

            while (isPathNotValid)
            {
                Console.WriteLine("Enter path to file:");
                var path = Console.ReadLine();
                Console.WriteLine("Enter file name:");
                var name = Console.ReadLine();
                
                try
                {
                    readText = File.ReadAllText(Path.Combine(path, name + endFileType));
                    isPathNotValid = false;
                }
                catch (Exception)
                {
                    ClearConsole();
                    Console.WriteLine("***Can't read that file!***\n");
                }
            }

            ClearConsole();
            Console.WriteLine("***File successful read!***\n");
            return readText;
        }

        public static void WriteTextToFile(string content, string endFileType)
        {
            ClearConsole();
            var isPathNotValid = true;

            while (isPathNotValid)
            {
                Console.WriteLine("Enter path to new file:");
                var path = Console.ReadLine();
                Console.WriteLine("Enter new file name:");
                var name = Console.ReadLine();

                try
                {
                    File.WriteAllText(Path.Combine(path, name + endFileType), content);
                    isPathNotValid = false;
                }
                catch (Exception)
                {
                    ClearConsole();
                    Console.WriteLine("***Can't write to that file!***\n");
                }
            }

            ClearConsole();
            Console.WriteLine("***File successful applied!***\n");
        }
    }
}