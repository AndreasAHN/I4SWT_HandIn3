using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T2_Powertube
    {

        private IPowertube _powertube;
        private Output _output;


        [SetUp]
        public void Setup()
        {
            _powertube = new Powertube();
            _output = new Output();
        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
