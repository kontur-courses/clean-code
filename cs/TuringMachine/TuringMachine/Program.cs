// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

namespace Turing
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var machine = new TuringMachine();
            Rule rule;
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;
                switch (line)
                {
                    case "start":
                        //TODO: start machine
                        break;
                    case "stop":
                        //TODO: stop machine
                        break;
                    case "step":
                        machine.NextStep();
                        continue;
                    case "settape":
                        Console.Write("Enter tape:");
                        var tape = Console.ReadLine();
                        machine.InsertTape(tape);
                        continue;
                    case "setinitstate":
                        Console.Write("Enter initial state:");
                        var state = Console.ReadLine();
                        machine.SetInitialState(state);
                        continue;
                    case "show":
                        var clip = machine.TapeClipping(0, 10);
                        Console.WriteLine(clip);
                        continue;
                    case "cls":
                        Console.Clear();
                        continue;
                    case "exit":
                        Environment.Exit(0);
                        break;
                }

                if (parser.TryParse(line, out rule))
                    machine.UpdateRules(rule);
            }
        }
    }
}