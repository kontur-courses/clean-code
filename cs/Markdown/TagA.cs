namespace Markdown
{
    public class TagA : Tag, IPairTag
    {
        private string link = "";
        private bool canEat = false;
        public TagA()
        {
            End = ")";
            Start = "[";
            FindRule = (token, previousToken, nextToken) => token == Start;
            CloseRule = (token, previousToken, nextToken) => token == End;
        }

        public override string ToString() => "a";

        public string StartTag => $"<a href=\"{link}\">";

        public string EndTag { get; } = "</a>";

        public bool CanContainTag(Tag tag) => true;


        public bool TryEat(string token)
        {
            if (token == ")")
                canEat = false;
            if (canEat && token != "(")
                link += token;
            if (token == "]")
                canEat = true;
            return canEat;
        }
    }
}
