using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MachineTests
    {
        private State startState;

        [SetUp]
        public void SetUp()
        {
            startState = State.Create();
        }

        [Test]
        public void Create_ShouldReturnNotNullValue()
        {
            Machine.Create(startState)
                .Should()
                .NotBeNull();
        }

        [Test]
        public void Run_ShouldThrowArgumentException_Null()
        {
            Action act = () => Machine.Create(startState).Run(null);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Input should be not null");
        }

        [Test]
        public void Run_ShouldThrowArgumentException_EmptyInput()
        {
            Action act = () => Machine.Create(startState).Run("");
            act.Should().Throw<ArgumentException>()
                .WithMessage("Input shouldn't be empty");
        }

        [Test]
        public void Run_ShouldCallOnEntryStateFunction_OtherState()
        {
            var stateCounter = 1;
            var nextState = State.Create().SetOnEntry((s, i) => stateCounter++);
            var machine = Machine.Create(startState);
            startState.SetFallback(nextState);
            machine.Run(" ");
            stateCounter.Should().Be(2);
        }

        [Test]
        public void Run_ShouldNotCallOnEntryStateFunction_TheSameState()
        {
            var stateCounter = 1;
            var machine = Machine.Create(startState);
            startState
                .SetOnEntry((s, i) => stateCounter++)
                .SetFallback(startState);
            machine.Run(" ");
            stateCounter.Should().Be(1);
        }

        [Test]
        public void Run_ShouldChangedToTransitionState_FallbackAndTransitionStates()
        {
            var flag = 0;
            var fallbackState = State.Create().SetOnEntry((s, i) => flag = 1);
            var transitionState = State.Create().SetOnEntry((s, i) => flag = 2);
            var machine = Machine.Create(startState);
            startState.AddTransition(' ', transitionState)
                .SetFallback(fallbackState);
            machine.Run(" ");
            flag.Should().Be(2);
        }

        [Test]
        public void Run_ShouldChangedToFallback_OnlyFallback()
        {
            var flag = 0;
            var fallbackState = State.Create().SetOnEntry((s, i) => flag = 1);
            var machine = Machine.Create(startState);
            startState.SetFallback(fallbackState);
            machine.Run(" ");
            flag.Should().Be(1);
        }

        [TestCase("a b c")]
        [TestCase("_da-wd-ac-aw-c")]
        [TestCase("123ldmaklw\n\n\n\t\\")]
        public void Run_ShouldCheckAllInput(string source)
        {
            var builder = new StringBuilder();
            var fallbackState = State.Create().SetOnEntry((s, i) => builder.Append(s[i]));
            var machine = Machine.Create(startState);
            startState.SetFallback(fallbackState).SetOnEntry((s, i) => builder.Append(s[i]));
            fallbackState.SetFallback(startState);
            machine.Run(source);
            builder.ToString().Should().Be(source);
        }
    }
}