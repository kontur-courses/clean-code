namespace Markdown.Tags.TagSpecification;

public class BulletedListTag() : BasicSingleTag(new Tag("- ", "<li>"), new Tag(Environment.NewLine, "</li>"))
{
    public override string PerformTagFormatting(TagReplacementSpecification replacement,
        BaseTag? previousTag, BaseTag? nextTag)
    {
        if (replacement.Tag == Opening)
            return previousTag is BulletedListTag ? replacement.Tag.New : "<ul>" + replacement.Tag.New;

        return nextTag is BulletedListTag ? replacement.Tag.New : replacement.Tag.New + "</ul>";
    }
}