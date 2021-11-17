using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawInput)
        {
            var replaceInfo = GetReplaceInfo(rawInput);
            var renderedInput = GetRenderedInput(rawInput, replaceInfo);
            return renderedInput;
        }

        private List<ReplaceInfo> GetReplaceInfo(string rawInput)
        {
            // Здесь будет алгоритм парсинга строки,
            // фиксирующий ReplaceInfo. Пока не понял, понадобятся ли ему
            // дополнительные методы, или будет логичнее, если он все сделает сам.
            // Кажется, благодаря тому, что внутри одинарных подчеркиваний "не работают"
            // двойные у нас не может быть ситуации, когда происходит многоуровневое вложение тегов,
            // что должно упростить парсинг, хотя я подозреваю, что всё равно будет
            // несколько подводных камней.
            throw new NotImplementedException();
        }

        private string GetRenderedInput(string rawInput, List<ReplaceInfo> replaceInfo)
        { 
            // Этот класс будет управлять циклом, который обходит список ReplaceInfo
            // и вызывать класс (вероятно, статический) или просто дополнительный метод,
            // который будет извлекать из rawInput подстроку на основе информации, полученной из
            // replaceInfo. Далее, подстроку будем добавлять в StringBuilder и возвращать его, приведённый
            // к строке, как результат. Извлечение подстроки работает за O(k), где k - длина подстроки,
            // добавление к StringBuilder так же, поэтому вроде бы пролазим по асимптотике.
            throw new NotImplementedException();
        }

        private string GetCorrectedSubstring(string rawInput, ReplaceInfo replaceInfo, int symbolsCount)
        {
            // Собственно, извлекатель подстроки, который попутно будет заменять Mark на Tag.
            throw new NotImplementedException();
        }
    }
}
