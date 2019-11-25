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
    public class T2_Powertube
    {

        private IPowerTube _powerTube;
        private Output _output;


        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _powerTube = new PowerTube(_output);
        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
