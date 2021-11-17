using System.Text;

namespace Markdown;
public class Md
{
    private readonly TokenWrapper tokenWrapper;
    private readonly string text;

    public Md(string text, WrapperSettingsProvider settings)
    {
        this.text = text;
        tokenWrapper = new TokenWrapper(settings);
    }

    public Md(string text)
    {
        this.text = text;
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("", "$(text)", "<p>$(text)</p>"));

        tokenWrapper = new TokenWrapper(settings);
    }


    public string Render()
    {
        var builder = new StringBuilder();
        foreach (var token in ParseLines())
        {
            var wrapped=tokenWrapper.WrapToken(token);
            builder.Append(wrapped.Render());
        }
        return builder.ToString();
    }

    private IEnumerable<Token> ParseLines()
    {
        foreach (var line in text.Split('\n'))
        {
            yield return new(line);
        }
    }

    private IEnumerable<Token> ParseLine(string line)
    {
        var tokenStack=new Stack<Token>();
        var tokenSeparators = tokenWrapper.Settings.Select(x => x.Key).ToArray();
        for (var i = 0; i < line.Length; i++)
        {
            foreach (var separator in tokenSeparators)
            {
                if (i+separator.Length<line.Length)
                {

                }
            }
        }
        return null;
    }
}
