
namespace Markdown.TokenSearcher
{
    public class MarkdownTokenSearcher : ITokenSearcher
    {
        public List<Token> FindTokens(string line)
        {
            var lines = SplitIntoLines(line);
            //Тут я планирую написать логику разбиения строки на токены.
            //Возможно придется добавить дополнительные методы
            throw new NotImplementedException();
        }

        private string[] SplitIntoLines(string line) {
            throw new NotImplementedException();
        }
    }
}
