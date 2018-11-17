using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Markdown.CmdInterface
{
    internal class CmdCallbacks
    {
        private IConverter converter;
        private string outputFilename = "output.txt";

        public string Convert(string inputFilename)
        {
            if (converter == null)
            {
                return "Выберите конвертер! /? для помощи";
            }

            if (!File.Exists(inputFilename))
            {
                return $"Файл {inputFilename} не найден";
            }

            using (var inputStream = new StreamReader(inputFilename, Encoding.Default))
            using (StreamWriter outputStream = new StreamWriter(File.Create(outputFilename), Encoding.Default))
            {
                string line;

                while ((line = inputStream.ReadLine()) != null)
                {
                    outputStream.WriteLine(converter.Convert(line));
                }
            }

            return $"Готово! См. файл: {outputFilename}";
        }

        public void SetOutputFile(string outputFilename)
        {
            this.outputFilename = outputFilename;
        }

        public string GetAvailableConvertersNames(IDictionary<string, IConverter> converters)
        {
            return string.Join("\n", converters.Keys);
        }

        public string SetConverter(string converterName, IDictionary<string, IConverter> converters)
        {
            if (!converters.TryGetValue(converterName.ToLower(), out converter))
            {
                return "Неправильное имя конвертера!";
            }

            return "";
        }

        public string GetHelpInformation()
        {
            return $@"===================
Справка по парсеру:
/help или /? – для открытия страницы помощи
/list converters – список доступных конвертеров
/output filename - имя файла, куда будет сохранен результат (сейчас установлен {outputFilename})
/convert filename [/output filename] /converter name - сконвертировать текст из файла `filename` в файл {outputFilename}
===================";
        }
    }
}