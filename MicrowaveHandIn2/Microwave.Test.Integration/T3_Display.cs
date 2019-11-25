using System;
using System.Collections.Generic;
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
        private IDisplay _display;
        private Output _output;



        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _display = new Display(_output);

        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
