using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown;

internal class WrappedToken
{
    private readonly Token token;
    private readonly TagSetting? setting;

    public WrappedToken(Token token, TagSetting? wrappingOption)
    {
        setting = wrappingOption;
        this.token=token;
    }

    public List<WrappedToken> Children { get; private set; } = new();

    public string Render()
    {
        var builder=new StringBuilder();
        foreach (var token in Children)
        {
            builder.Append(token.Render());
        }

        if (setting != null)
            builder.Append(setting.Render(token.Text));
        else
            builder.Append(token.Text);

        return builder.ToString();
    }
}
