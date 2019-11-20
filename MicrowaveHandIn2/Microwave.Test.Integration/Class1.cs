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
    public class Class1
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
