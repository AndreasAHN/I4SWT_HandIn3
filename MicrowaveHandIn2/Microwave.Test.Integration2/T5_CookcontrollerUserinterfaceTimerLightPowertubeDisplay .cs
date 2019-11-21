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
    public class T5_CookcontrollerUserinterfaceTimerLightPowertubeDisplay
    {
        private ILight _light;
        private IOutput _output;
        private ICookcontroller _cookcontroller;
        private IDisplay _display;
        private IPowertube _powertube;
        private ITimer _timer;
        private IUserinterface _userinterface;




        [SetUp]
        public void Setup()
        {
            _light = new Light();
            _output = new Output();
            _cookcontroller = new Cookcontroller();
            _display = new Display();
            _powertube = new Powertube();
            _timer = new Timer();
            _userinterface = new Userinterface();
        }

        [Test]
        public void SimpleIntegrationTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
