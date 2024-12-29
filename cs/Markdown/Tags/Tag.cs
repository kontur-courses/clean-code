using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Markdown.Tags;

public abstract class Tag(string MarkdownText, int TagStart)
{
    private static readonly Dictionary<MdTagType, HashSet<MdTagType>> nestedRules = new()
    {
        { MdTagType.Header, new () { MdTagType.Bold, MdTagType.Italic, MdTagType.Escape}},
        { MdTagType.Bold, new () { MdTagType.Italic, MdTagType.Escape}},
        { MdTagType.Italic, new () { MdTagType.Escape} } 
    };

    public static bool IsNestedTagWorks(MdTagType external, MdTagType nested)
    {
        return nestedRules.ContainsKey(external) && nestedRules[external].Contains(nested);
    }

    public bool IsTagClosed { get; protected set; }
    public int TagStart { get;  } = TagStart;
    public int TagEnd;
    public string MarkdownText = MarkdownText;
    public abstract MdTagType TagType { get; }
    private Token? context;
    protected List<Tag>? NestedTags;
    protected bool IsContextCorrect = true;
    protected Token Context
    {
        get => context ?? throw new IncompleteInitialization();
        set
        {
            if (!IsTagClosed)
            {
                IsTagClosed = true;
                context = value;
            }
        }
    }
    protected abstract string MdTag { get; }
    protected abstract string HtmlTag { get; }
    
    public virtual int SkipTag(int position) => position + MdTag.Length;
    public virtual string RenderToHtml()
    {
        var result = new StringBuilder();
        var tags = NestedTags?.ToDictionary(t => t.TagStart) ?? [];
        for (var i = TagStart + MdTag.Length; i < Math.Min(Context.Position + Context.Length, MarkdownText.Length); )
        {
            if (tags.ContainsKey(i))
            {
                var nested = tags[i];
                result.Append(nested.RenderToHtml());
                i = nested.TagEnd;
            }
            else
            {
                result.Append(MarkdownText[i]);
                i++;
            } 
        }
        return $"<{HtmlTag}>{result}</{HtmlTag}>";
    }
    public virtual void TryCloseTag(int contextEnd, string sourceMdText, out int tagEnd, List<Tag>? nested = null)
    {
        var contextStart = TagStart + MdTag.Length;
        tagEnd = contextEnd + MdTag.Length;
        Context = new Token(contextStart, sourceMdText, contextEnd - contextStart);
        TagEnd  = tagEnd;
        NestedTags = nested;
    }

    public abstract bool AcceptIfContextEnd(int currentPosition);
    public virtual bool AcceptIfContextCorrect(int currentPosition) => true;
}