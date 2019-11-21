namespace Markdown
{
    class Md
    {
        public string Render(string paragraph)
        {
            var analyzer = new LexicalAnalyzer();
            var tokens = analyzer.Analyze(paragraph);
            var parser = new TokenParser(tokens, paragraph);
            var tags = parser.Parse();
            var tagInserter = new TagInserter();
            var HTMLtext = tagInserter.Insert(paragraph, tags);
            return HTMLtext;
        }

    }

}
