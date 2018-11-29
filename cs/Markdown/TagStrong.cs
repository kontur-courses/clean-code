namespace Markdown
{
    public class TagStrong : Tag, IPairTag
    {
        public TagStrong()
        {
            End = "__";
            Start = End;
            IsAgreedStart = (token, previousToken, nextToken) => (token == Start && !nextToken.StartsWith(" "));
            IsAgreedEnd = (token, previousToken, nextToken) => (token == End && !previousToken.EndsWith(" "));
        }

        public override string ToString() => "strong";

        public string StartTag { get; } = "<strong>";

        public string EndTag { get; } = "</strong>";

        public bool CanContainTag(Tag tag)=> true;

        public bool TryEat(string tag)=> false;
    }
}
