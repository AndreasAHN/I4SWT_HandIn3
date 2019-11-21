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
    public class T1_Light
    {
        private ILight _light;
        private Output _output;



        [SetUp]
        public void Setup()
        {
            _light = new Light();
            _output = new Output();
        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
