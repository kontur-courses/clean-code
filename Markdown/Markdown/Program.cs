using System;
using System.IO;
using System.Text;
using Markdown.CoreParser;
using Markdown.CoreParser.ConverterInTokens;
using Markdown.Tokens;
using Markdown.Transducer.ConverterTokenToHtml;

namespace Markdown
{
    internal class Program
    {
        private static bool IsFileNameValid(string fileName)
        {
            if ((fileName == null) || (fileName.IndexOfAny(Path.GetInvalidPathChars()) != -1))
                return false;
            try
            {
                var tempFileInfo = new FileInfo(fileName);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }            
        }

        private static string ReadFile(string path)
        {
            using ( var fileStream = File.OpenRead(path))
            {
                var arrayByte = new byte[fileStream.Length];
                fileStream.Read(arrayByte, 0, arrayByte.Length);
                return Encoding.Default.GetString(arrayByte);
            }
        }

        private static void WriteFile(string path, string text)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var input = Encoding.Default.GetBytes(text);
                fileStream.Write(input, 0, input.Length);
                Console.WriteLine("Обработанный текст записан в файл " + path);
            }
        }
        
        public static void Main(string[] args)
        {
            if (args.Length == 0 || !IsFileNameValid(args[0]))
                throw new ArgumentException($"не правильный путь в качестве 1 аргумента");
            
            var path = args[0];
            var str = ReadFile(path);

            var parser = new Parser();
            var singleEmphasis = new SingleEmphasis();
            var doubleEmphasis = new DoubleEmphasis();
            doubleEmphasis.RegisterNested(singleEmphasis);
            parser.Register(singleEmphasis);
            parser.Register(doubleEmphasis);
            
            var transducer = new Transducer.Transducer();
            var converterSingleEmphasis = new ConverterSingleEmphasis();
            var converterDoubleEmphasis = new ConverterDoubleEmphasis();
            converterSingleEmphasis.RegisterNested(new DoubleEmphasisToken("", 0, new IToken[0]), converterDoubleEmphasis);
            converterDoubleEmphasis.RegisterNested(new SingleEmphasisToken("", 0, new IToken[0]), converterSingleEmphasis);
            transducer.Registred(new SingleEmphasisToken("", 0, new IToken[0]), converterSingleEmphasis);
            transducer.Registred(new DoubleEmphasisToken("", 0, new IToken[0]), converterDoubleEmphasis);
            
            WriteFile(Path.Combine(Path.GetDirectoryName(path), "output.txt"), transducer.MakeHtmlString(str, parser.Tokenize(str)));
        }
    }
}