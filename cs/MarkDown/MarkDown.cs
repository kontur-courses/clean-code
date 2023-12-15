using System.Text;
using MarkDown.Interfaces;

namespace MarkDown;

public class MarkDown
{
    private readonly MarkDownEnvironment markDownEnvironment;
    private readonly string text;
    private ITagContext nowContext;

    public MarkDown(string text, MarkDownEnvironment markDownEnvironment)
    {
        this.markDownEnvironment = markDownEnvironment;
        this.text = text;
    }
    
    public string GenerateHtml()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < text.Length; i++)
        {
            
        }
        
        throw new NotImplementedException();
    }
}