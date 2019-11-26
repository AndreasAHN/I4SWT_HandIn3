using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

        // To Test Console output
        private StringWriter _stringWriter;


        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new Light(_output);

            // Set Console.SetOut(_stringWriter);
            // Console is now writing to a StringWriter instead of actual console.
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [Test]
        public void TurnOn_OutputCorrect()
        {
            _sut.TurnOn();

            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }

        [Test]
        public void TurnOff_OutputCorrect()
        {
            _sut.TurnOn();
            _sut.TurnOff();

            StringAssert.IsMatch("Light is turned off", _stringWriter.ToString());
        }

        [Test]
        public void TurnOffWhileOff_OutputNothing()
        {
            _sut.TurnOff();

            StringAssert.IsMatch(string.Empty, _stringWriter.ToString());
        }

        [Test]
        public void TurnOnWhileOn_OutputNothing2ndTime()
        {
            _sut.TurnOn();
            _sut.TurnOn();

            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }

        [Test]
        public void TurnOnOffOff_OutputOneOff()
        {
            _sut.TurnOn();
            _sut.TurnOff();
            _sut.TurnOff();

            // Two outputs written! new line is needed.
            StringAssert.IsMatch("Light is turned on\r\nLight is turned off", _stringWriter.ToString());
        }
    }
}
