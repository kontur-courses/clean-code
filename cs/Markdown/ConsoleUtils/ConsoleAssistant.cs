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
                .ToDictionary(x => x.Name.ToLower().Substring(0, x.Name.Length - 9));
        }


        public static TokenConverter SetupConverter()
        {
            string key;
            var converterTypes = GetAllConverterTypes();

            while (true)
            {
                Console.WriteLine("Enter final convert type of file:");
                key = Console.ReadLine();

                if(key != null && 
                   !string.IsNullOrEmpty(converterTypes.Keys.FirstOrDefault(x => x.Equals(key))))
                    break;
                    
                Console.WriteLine("\n***That type doesn't included in types library.***\n");
            }

            Console.WriteLine("\n***Type successful applied!***\n");

            var type = converterTypes.Keys.First(x => x.StartsWith(key));
            return (TokenConverter)Activator.CreateInstance(converterTypes[type]);
        }

        public static string ReadTextFromFile(string endFileType)
        {
            var isPathNotValid = true;
            string readText = null;

            while (isPathNotValid)
            {
                Console.WriteLine("Enter path to file:");
                var path = Console.ReadLine();
                if(string.IsNullOrEmpty(path))
                    continue;

                Console.WriteLine("Enter file name:");
                var name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                    continue;

                try
                {
                    readText = File.ReadAllText(Path.Combine(path, name + endFileType));
                    isPathNotValid = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n***Can't read that file!***");
                    Console.WriteLine($"***Error: {e.Message}***\n");
                }
            }

            Console.WriteLine("\n***File successful read!***\n");
            return readText;
        }

        public static void WriteTextToFile(string content, string endFileType)
        {
            var isPathNotValid = true;

            while (isPathNotValid)
            {

                Console.WriteLine("Enter path to new file:");
                var path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                    continue;

                Console.WriteLine("Enter new file name:");
                var name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                    continue;

                try
                {
                    File.WriteAllText(Path.Combine(path, name + endFileType), content);
                    isPathNotValid = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n***Can't write to that file!***");
                    Console.WriteLine($"***Error: {e.Message}***\n");
                }
            }

            Console.WriteLine("\n***File successful applied!***\n");
        }
    }
}