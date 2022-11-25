using System.Text;

namespace Markdown;

public class ReplaceTagInspectorConfig
{
    public StringBuilder Text;
    public TagConfiguration TagConfiguration;
    public int Index;
    public List<Func<int, bool>> CustomConditions;
    public bool NeedClosure;

    public ReplaceTagInspectorConfig(StringBuilder text, TagConfiguration tagConfiguration, int index, List<Func<int, bool>> customConditions, bool needClosure)
    {
        Text = text; 
        TagConfiguration = tagConfiguration;
        Index = index;
        CustomConditions = customConditions;
        NeedClosure = needClosure;
    }
}