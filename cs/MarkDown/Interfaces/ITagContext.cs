using System.Text;

namespace MarkDown.Interfaces;

public interface ITagContext
{
    public int StartIndex { get; }
    public void DeleteUnsupportedInners(MarkDownEnvironment environment, StringBuilder sb);
}