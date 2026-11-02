using System.Text;

namespace Markdown;

public interface INode
{
    void Render(StringBuilder sb);
}