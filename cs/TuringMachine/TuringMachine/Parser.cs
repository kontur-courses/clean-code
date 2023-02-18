using System.Text.RegularExpressions;

namespace Turing;

public sealed class Parser
{
    public bool TryParse(string expression, out Rule rule)
    {
        rule = new Rule();
        var expressionRegex = new Regex(@"\s*\w+\d*\s\w\s*->\s*\w+\d*\s\w\s[LRN]\s*");
        if (!expressionRegex.IsMatch(expression))
        {
            Console.WriteLine("Invalid expression"); //TODO:enum синтаксических ошибок
            return false;
        }

        var conditions = expression
            .Split("->")
            .SelectMany(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        var opState = conditions[0];
        var clState = conditions[2];
        var opLetter = conditions[1][0];
        var clLetter = conditions[3][0];
        var action = conditions[4][0];

        rule = new Rule(opState, clState, opLetter, clLetter, action);
        return true;
    }
}