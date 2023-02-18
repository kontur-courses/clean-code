using FluentAssertions;
using Turing;

namespace TestProject1;

public class TuringMachineTests
{
    private Parser parser;
    private TuringMachine machine;

    [SetUp]
    public void Setup()
    {
        parser = new();
        machine = new();
    }

    private void SetupMachine(string[] rulesExpressions)
    {
        var rules = new List<Rule>();
        Rule rule;
        foreach (var ruleExpr in rulesExpressions)
        {
            if (parser.TryParse(ruleExpr, out rule))
            {
                rules.Add(rule);
            }
        }

        machine.UpdateRules(rules);
    }

    [TestCase("")]
    [TestCase("a")]
    [TestCase("aa")]
    public void InsertTapeTest(string tape)
    {
        machine.InsertTape(tape);
        machine
            .TapeClipping(tape.Length)
            .Should()
            .Be(tape);
    }

    [TestCase("ab", "q0", "ba", 0, 2, 2, "q0 a->q0 b R", "q0 b->q0 a R")]
    public void Test(string tape, string initialState, string result, int startPosition, int endPosition,
        int iterations, params string[] ruleExpressions)
    {
        SetupMachine(ruleExpressions);
        machine.SetInitialState(initialState);
        machine.InsertTape(tape);
        machine.DoSteps(iterations);

        machine
            .TapeClipping(startPosition, endPosition)
            .Should()
            .Be(result);
    }
}