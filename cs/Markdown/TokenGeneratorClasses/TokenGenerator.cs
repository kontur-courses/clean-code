using System.Reflection;
using Markdown.TokenGeneratorClasses.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses;

public class TokenGenerator : ITokenGenerator
{
    private static readonly IEnumerable<ITokenGenerateRule> generateRuleClasses = GetRuleClasses();

    public static Token? GetToken(string line, int currentIndex)
    {
        foreach (var rule in generateRuleClasses)
        {
            var token = rule.GetToken(line, currentIndex);
            if (token != null)
                return token;
        }

        return null;
    }

    private static IEnumerable<ITokenGenerateRule> GetRuleClasses()
    {
        var interfaceType = typeof(ITokenGenerateRule);

        var rulesTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass)
            .ToHashSet();

        var simpleRules = GetRulesNotUsesOthersRules(rulesTypes).ToList();
        var complexRules = GetRulesUsesOthersRulesLogic(rulesTypes, simpleRules).ToList();

        return simpleRules.Concat(complexRules);
    }

    private static IEnumerable<ITokenGenerateRule> GetRulesNotUsesOthersRules(HashSet<Type> rulesTypes)
    {
        foreach (var type in rulesTypes)
        {
            var constructors = type.GetConstructors();

            if (constructors.Length == 1 && constructors[0].GetParameters().Length == 0)
            {
                rulesTypes.Remove(type);
                yield return (ITokenGenerateRule)Activator.CreateInstance(type);
            }
        }
    }

    private static IEnumerable<ITokenGenerateRule> GetRulesUsesOthersRulesLogic(HashSet<Type> rulesTypes,
        IEnumerable<ITokenGenerateRule> rulesNotUsesOthersRules)
    {
        var getTokenFuncs = rulesNotUsesOthersRules
            .Select(rule => new Func<string, int, Token?>(rule.GetToken));

        foreach (var type in rulesTypes)
        {
            var constructor = type.GetConstructor(new[] { typeof(IEnumerable<Func<string, int, Token?>>) });

            if (constructor == null)
                throw new ArgumentNullException("TokenGeneratorRules should have only one constructor " +
                                                "without arguments or with IEnumerable<ITokenGenerateRule> argument");
            rulesTypes.Remove(type);
            yield return (ITokenGenerateRule)constructor.Invoke(new object[] { getTokenFuncs });
        }
    }
}