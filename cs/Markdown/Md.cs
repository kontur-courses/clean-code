namespace Markdown;

    public class Md
    {
        public string Render(string input)
        {
            var tokens = new TokenCreator().CreateTokens(input);
            var nodeTree = new MarkdownParser().Parse(tokens);
            var render =  new HtmlRenderer().Render(nodeTree);
            return render;
        }
    }