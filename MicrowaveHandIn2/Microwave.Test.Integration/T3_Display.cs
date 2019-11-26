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
    public class T3_Display
    {
        private IDisplay _sut;
        private IOutput _output;

        private StringWriter _stringWriter;


        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new Display(_output);

            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [TestCase(1, 3)]
        [TestCase(0, 30)]
        [TestCase(10, 13)]
        public void ShowTime_ShowsCorrectTime(int min, int sec)
        {
            _sut.ShowTime(min, sec);
            StringAssert.IsMatch($"Display shows: {min:D2}:{sec:D2}", _stringWriter.ToString());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(700)]
        [TestCase(701)]
        public void ShowPower_ShowsCorrectPowerInWatt(int power)
        {
            _sut.ShowPower(power);
            StringAssert.IsMatch($"Display shows: {power} W", _stringWriter.ToString());
        }

        [Test]
        public void Clear_ShowsDisplayCleared()
        {
            _sut.Clear();
            StringAssert.IsMatch($"Display cleared", _stringWriter.ToString());
        }
    }
}
