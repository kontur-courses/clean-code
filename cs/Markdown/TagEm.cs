namespace Markdown
{
    public class TagEm : Tag, IPairTag
    {
        public TagEm()
        {
            End = "_";
            Start = End;
            FindRule = (token, previousToken, nextToken) => (token == Start && !nextToken.StartsWith(" "));
            CloseRule = (token, previousToken, nextToken) => (token == End && !previousToken.EndsWith(" "));
        }

        public override string ToString() => "em";

        public string StartTag { get; } = "<em>";

        public string EndTag { get; } = "</em>";

        public bool CanContainTag(Tag tag)=>!(tag is TagStrong);        

        public bool TryEat(string tag) => false;
    }
}
