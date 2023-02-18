using System.Collections;

namespace Turing;

sealed class Rules : IEnumerable<Rule>
{
    private List<Rule> data;

    public Rules()
    {
        data = new();
    }

    public void Add(Rule rule) => data.Add(rule);

    public Rule this[string state, char letter]
    {
        get
        {
            var result =
                data.FirstOrDefault(rule =>
                    rule.OpeningState == state && rule.OpeningLetter == letter); //TODO: or default
            if (result.OpeningState is null) //TODO: сделать нормальное определение default значение
                return new Rule(state, state, letter, letter, 'N');
            return result;
        }
    }

    public void AddRange(IEnumerable<Rule> rules) =>
        data.AddRange(rules);

    public IEnumerator<Rule> GetEnumerator()
    {
        foreach (var rule in data)
            yield return rule;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}