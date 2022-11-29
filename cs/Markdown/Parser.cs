using Markdown.Types;

namespace Markdown;

public class Parser
{
    private Dictionary<string, IType> typeParsers = new Dictionary<string, IType>()
    {
        { "_", new Italy() },
        { "__", new Strong() },
    };

    public List<ParsedString> Parse(string text)
    {
        var stackTypes = new Stack<ParsedString>();
        var required = new List<ParsedString>();
        for (int i = 0; i < text.Length; i++)
        {
            string prefix = GetPrefix(text, i);
            if (prefix == "__") i++;
            
            if (stackTypes.Count > 0 && !prefix.Equals("") && typeParsers[prefix] == stackTypes.Peek().Type)
            {
                var item = stackTypes.Pop();
                item.End = i - prefix.Length + 1;
                required.Add(item);
                continue;
            }
            if (stackTypes.Count > 0 && stackTypes.Peek().Type == typeParsers["_"])
                continue;
            if (typeParsers.Keys.Contains(prefix))
            {
                if (stackTypes.Count == 1 && stackTypes.Peek().Type.GetType() == new NoneType().GetType())
                {
                    var item = stackTypes.Pop();
                    item.End = i;
                    required.Add(item);
                }

                stackTypes.Push(new ParsedString(typeParsers[prefix], i + 1, prefix));
            }
            if (stackTypes.Count == 0) stackTypes.Push(new ParsedString(new NoneType(), i, ""));
        }
        if (stackTypes.Count == 1 && stackTypes.Peek().Type.GetType() == new NoneType().GetType())
        {
            var item = stackTypes.Pop();
            item.End = text.Length;
            required.Add(item);
        }
        return required;
    }

    private string GetPrefix(string text, int index)
    {
        string prefix = "";
        char letter = text[index];
        if (index + 1 < text.Length && letter == '_' && text[index + 1] == '_')
        {
            prefix = "__";
        }
        else if (typeParsers.Keys.Contains(letter.ToString()))
            prefix = letter.ToString();

        return prefix;
    }
}