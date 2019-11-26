using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T2_Powertube
    {
        private IPowerTube _sut;
        private IOutput _output;

        // To Test Console output
        private StringWriter _stringWriter;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new PowerTube(_output);

            // Set Console.SetOut(_stringWriter);
            // Console is now writing to a StringWriter instead of actual console.
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(99)]
        [TestCase(100)]
        public void InBoundTest_PowerTubeTurnedOn(int arg)
        {
            _sut.TurnOn(arg);

            string str = $"PowerTube works with {arg} %";

            StringAssert.IsMatch(str, _stringWriter.ToString());
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(101)]
        [TestCase(102)]
        public void OutOfBoundTest_ThrowsArgumentOutOfRangeException(int arg)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.TurnOn(arg));
        }

        [Test]
        public void PowerTubeIsOn_ThrowsException()
        {
            _sut.TurnOn(40);
            Assert.Throws<ApplicationException>(() => _sut.TurnOn(50));
        }

        [Test]
        public void PowerTubeIsOn_TurnOffCorrect()
        {
            int power = 50;
            _sut.TurnOn(power);
            _sut.TurnOff();

            StringAssert.IsMatch($"PowerTube works with { power } %\r\nPowerTube turned off", _stringWriter.ToString());
        }

        [Test]
        public void PowerTubeIsOff_TurnOff_NothingWrittenToOutput()
        {
            _sut.TurnOff();

            StringAssert.IsMatch(string.Empty, _stringWriter.ToString());
        }
    }
}
