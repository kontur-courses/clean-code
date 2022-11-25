using System.Text;

namespace Markdown;

public class ReplaceTagInspectorConfig
{
    public readonly StringBuilder Text;
    public readonly TagConfiguration TagConfiguration;
    public readonly int Index;
    public readonly List<Func<int, bool>> CustomConditions;
    public readonly bool NeedClosure;

    public ReplaceTagInspectorConfig(StringBuilder text, TagConfiguration tagConfiguration, int index, List<Func<int, bool>> customConditions, bool needClosure)
    {
        Text = text; 
        TagConfiguration = tagConfiguration;
        Index = index;
        CustomConditions = customConditions;
        NeedClosure = needClosure;
    }
}