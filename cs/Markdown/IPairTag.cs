namespace Markdown
{
    internal interface IPairTag
    {
        string StartTag { get; }

        string EndTag { get; }

        bool CanContainTag(Tag tag);

        bool TryEat(string tag);
    }
}
