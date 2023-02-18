using FluentAssertions;
using Turing;

namespace TestProject1;

public class ParserTests
{
    private Parser parser;

    [SetUp]
    public void Setup()
    {
        parser = new Parser();
    }

    [TestCase("q0 a->q0 b R")]
    [TestCase("q a->q b L")]
    [TestCase("q a->q0 b N")]
    public void Test(string ruleExpression)
    {
        Rule rule;
        if (!parser.TryParse(ruleExpression, out rule))
            throw new Exception();
        var ruleProperties = ruleExpression
            .Split("->")
            .SelectMany(s => s.Split())
            .ToArray();
        rule
            .Should()
            .Be(new Rule(
                ruleProperties[0],
                ruleProperties[2],
                ruleProperties[1][0],
                ruleProperties[3][0],
                ruleProperties[4][0])
            );
    }
}