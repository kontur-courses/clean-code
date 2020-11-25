using System;

namespace Markdown
{
    /// <summary>
    /// класс содержит символ идентифицирующий тэг, все символы тэга и метод для преобразования тэга
    /// </summary>
    public abstract class Mark
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
       // public Func<string, string> TransformMark { get; }

        public Mark(string definingSymbol, string[] allSymbols)
        {
            DefiningSymbol = definingSymbol;
            AllSymbols = allSymbols;
        }

        public virtual string TransformMark(string text)
        {
            throw new NotImplementedException();
        }
    }
}