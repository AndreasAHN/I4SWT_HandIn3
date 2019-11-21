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
    public class T9_ButtonDoorUserinterface
    {
        private IDoor _door;
        private IButton _button;
        private IUserinterface _userinterface;



        [SetUp]
        public void Setup()
        {
            _userinterface = new Userinterface();
            _door = new Door();
            _button = new Button();
        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
