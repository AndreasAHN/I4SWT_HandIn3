using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T9_ButtonDoorUserinterface
    {
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;

        private IDoor _door;
        private IOutput _output;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ILight _light;
        private ITimer _timer;
        private ICookController _cookController;
        private IUserInterface _userinterface;
        
        private StringWriter _stringWriter;


        [SetUp]
        public void Setup()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timeButton = new Button();

            _door = new Door();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userinterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);

            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [Test]
        public void DoorClosed_Just_PowerButtonPressTest()
        {
            _door.Close();
            _powerButton.Press();
            StringAssert.IsMatch("Display shows: 50 W", _stringWriter.ToString());
        }

        [Test]
        public void DoorClosed_Just_StartCancelButtonPressTest()
        {
            _door.Close();
            _startCancelButton.Press();
            Assert.IsEmpty(_stringWriter.ToString());
        }

        [Test]
        public void DoorClosed_Just_TimeButtonButtonPressTest()
        {
            _door.Close();
            _timeButton.Press();
            Assert.IsEmpty(_stringWriter.ToString());
        }

        [Test]
        public void DoorClosed_StartCancelAndPowerButtonPressTest()
        {
            _door.Close();
            _powerButton.Press();
            _startCancelButton.Press();
            StringAssert.IsMatch("Display cleared", _stringWriter.ToString());
        }

        [Test]
        public void DoorClosed_TimeAndPowerButtonPressTest()
        {
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            StringAssert.IsMatch("Display shows: 01:00", _stringWriter.ToString());
        }


        [Test]
        public void DoorOpen_Just_PowerButtonPressTest()
        {
            _door.Open();
            _powerButton.Press();
            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }

        [Test]
        public void DoorOpen_Just_StartCancelButtonPressTest()
        {
            _door.Open();
            _startCancelButton.Press();
            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }

        [Test]
        public void DoorOpen_Just_TimeButtonButtonPressTest()
        {
            _door.Open();
            _timeButton.Press();
            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }

        [Test]
        public void DoorOpen_StartCancelAndPowerButtonPressTest()
        {
            _door.Open();
            _powerButton.Press();
            _startCancelButton.Press();
            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }

        [Test]
        public void DoorOpen_TimeAndPowerButtonPressTest()
        {
            _door.Open();
            _powerButton.Press();
            _timeButton.Press();
            StringAssert.IsMatch("Light is turned on", _stringWriter.ToString());
        }
    }
}
