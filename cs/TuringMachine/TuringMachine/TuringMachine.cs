using System.Text;

namespace Turing;

public sealed class TuringMachine
{
    private const int TapeMaxLenght = 65536;

    private HashSet<char> alphabet;
    private HashSet<string> states;
    private string currentState;
    private Rules _rules;
    private int pointer;
    private char[] tape;

    public TuringMachine(string initialState = null)
    {
        pointer = 0;
        currentState = initialState;
        tape = new char[TapeMaxLenght];
        _rules = new();
        states = new();
        alphabet = new();
    }

    public TuringMachine(IEnumerable<Rule> rules, string initialState = null) : this(initialState)
    {
        _rules.AddRange(rules);
    }

    private void Move(int offset)
    {
        pointer = (pointer + offset) % TapeMaxLenght;
    }

    private void MoveLeft() => Move(-1);
    private void MoveRight() => Move(1);

    private char CurrentCell
    {
        get => tape[pointer];
        set => tape[pointer] = value;
    }

    public void SetInitialState(string state)
    {
        if (!(currentState is null))
            throw new ArgumentException("Machine already initialize current state");
        currentState = state;
    }

    public void InsertTape(string tapeClip)
    {
        for (int i = 0; i < tapeClip.Length; i++)
        {
            tape[(pointer + i) % TapeMaxLenght] = tapeClip[i];
        }
    }

    public void UpdateRules(Rule rule)
    {
        Update(rule);

        _rules.Add(rule);
    }

    public void UpdateRules(IEnumerable<Rule> rules)
    {
        foreach (var rule in rules)
        {
            UpdateRules(rule);
        }
    }

    private void Update(Rule rule)
    {
        states.Add(rule.OpeningState);
        states.Add(rule.ClosingState);

        alphabet.Add(rule.OpeningLetter);
        alphabet.Add(rule.ClosingLetter);
    }

    public void NextStep()
    {
        if (currentState is null)
            throw new Exception("Current state is undefined");
        var rule = _rules[currentState, CurrentCell];
        currentState = rule.ClosingState;
        CurrentCell = rule.ClosingLetter;
        switch (rule.Action)
        {
            case 'L':
                MoveLeft();
                break;
            case 'R':
                MoveRight();
                break;
        }
    }

    public void DoSteps(int count)
    {
        for (int i = 0; i < count; i++)
        {
            NextStep();
        }
    }

    public string TapeClipping(int lenght) => TapeClipping(pointer, pointer + lenght);

    public string TapeClipping(int startPosition, int endPosition)
    {
        if (startPosition > endPosition)
            throw new ArgumentException("");
        var builder = new StringBuilder();
        for (int i = startPosition; i < endPosition; i++)
        {
            builder.Append(tape[i % TapeMaxLenght]);
        }

        return builder.ToString();
    }
}