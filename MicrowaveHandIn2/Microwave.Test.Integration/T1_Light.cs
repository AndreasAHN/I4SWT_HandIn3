using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute.Core;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T1_Light
    {
        private IOutput _output;
        private ILight _sut;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _sut = new Light(_output);
        }

        [Test]
        public void TurnOn_OutputCorrect()
        {
            _sut.TurnOn();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("on")));
        }

        [Test]
        public void TurnOff_OutputCorrect()
        {
            _sut.TurnOn();
            _sut.TurnOff();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("off")));
        }

        [Test]
        public void TurnOffWhileOff_OutputNothing()
        {
            _sut.TurnOff();

            _output.Received(0).OutputLine(Arg.Is<string>(x => x.Contains("off")));
        }

        [Test]
        public void TurnOnWhileOn_OutputNothing2ndTime()
        {
            _sut.TurnOn();
            _sut.TurnOn();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("on")));
        }

        [Test]
        public void TurnOnOffOff_OutputOneOff()
        {
            _sut.TurnOn();
            _sut.TurnOff();
            _sut.TurnOff();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains("off")));
        }
    }
}
