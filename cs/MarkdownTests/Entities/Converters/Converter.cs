using Markdown.Entities.Builders;
using Markdown.Entities.Parsers;
using Markdown.Entities.Renderers;
using Markdown.Models.SyntaxTree;

namespace Markdown.Entities.Converters
{
    /// <summary>
    /// Выполняет полный цикл преобразования одного языка разметки в другой.
    /// Координирует работу токенизатора и билдера.
    /// Конструктор позволяет определить желаемый билдер и токенизатор под текущую задачу, например
    /// в нашем случае токенизатор умеющий работать с markdown-разметкой а buidler который умеет строить
    /// по синтаксическому дереву исходный текст с html-разметкой
    /// </summary>
    /// <param name="text">Исходный Markdown-текст</param>
    /// <returns>Строка с HTML-разметкой</returns>
    /// <remarks>
    /// Последовательность преобразований:
    /// 1. Токенизация исходного текста
    /// 2. Построение абстрактного синтаксического дерева (AST)
    /// 3. Рендеринг AST в HTML-формат
    /// </remarks>
    public class Converter : IConverter
    {
        private readonly IBuilder builder;
        private readonly ITokenizer tokenizer;

        public Converter(IBuilder builder, ITokenizer tokenizer)
        {
            this.builder = builder;
            this.tokenizer = tokenizer;
        }

        public string Convert(string text)
        {
            if (builder == null) throw new NullReferenceException("Builder can't be null");
            if (tokenizer == null) throw new NullReferenceException("Tokenizer can't be null");
            
            var tokens = tokenizer.Tokenize(text); 
            var ast = new SyntaxTree(tokens);
            var convertedText = builder.Build(ast);

            return convertedText;
        }
    }
}
