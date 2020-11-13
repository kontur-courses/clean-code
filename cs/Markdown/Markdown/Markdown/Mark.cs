using System;

namespace Markdown
{
    /// <summary>
    /// класс содержит символ индетифицирующий тэг, все символы тэга и функцию для преобразования тэга
    /// </summary>
    public class Mark
    {
        /// <summary>
        /// строка определяющая начало символа 
        /// </summary>
        public string DefiningSymbol { get; }
        /// <summary>
        /// все символы по порядку их использования требуемые для преобразования тэга
        /// </summary>
        public string[] AllSymbols { get; }

        /// <summary>
        /// функция для правильного преобразования тэга 
        /// </summary>
        public Func<string, string> TransformMark { get; }

        public Mark(string definingSymbol, string[] allSymbols, Func<string, string> transformMark)
        {
            DefiningSymbol = definingSymbol;
            AllSymbols = allSymbols;
            TransformMark = transformMark;
        }
        
        public Mark(string definingSymbol)
        {
            DefiningSymbol = definingSymbol;
        }
    }
}