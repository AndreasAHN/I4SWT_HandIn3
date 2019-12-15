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

        [TestCase("Display shows: 50 W")]
        [Test]
        public void DoorClosed_Just_PowerButtonPress_Test(string result)
        {
            _door.Close();
            _powerButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [Test]
        public void DoorClosed_Just_StartCancelButtonPress_Test()
        {
            _door.Close();
            _startCancelButton.Press();
            Assert.IsEmpty(_stringWriter.ToString());
        }

        [Test]
        public void DoorClosed_Just_TimeButtonButtonPress_Test()
        {
            _door.Close();
            _timeButton.Press();
            Assert.IsEmpty(_stringWriter.ToString());
        }

        [TestCase("Display shows: 50 W")]
        [TestCase("Display cleared")]
        [Test]
        public void DoorClosed_StartCancelAndPowerButtonPress_Test(string result)
        {
            _door.Close();
            _powerButton.Press();
            _startCancelButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Display shows: 50 W")]
        [TestCase("Display shows: 01:00")]
        [Test]
        public void DoorClosed_TimeAndPowerButtonPress_Test(string result)
        {
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("PowerTube works with")]
        [TestCase("PowerTube turned off")]
        [TestCase("Display shows: 00:00")]
        [Test]
        public void DoorClosed_RunnigMicrowave_Test(string result)
        {
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            var constraint = Is.True.After(62000);
            Assert.That(() => true, constraint);

            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("PowerTube works with")]
        [TestCase("Light is turned on")]
        [TestCase("PowerTube turned off")]
        [TestCase("Light is turned off")]
        [TestCase("Display cleared")]
        [Test]
        public void DoorClosed_RunnigMicrowaveAndCanceling_Test(string result)
        {
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            var constraint = Is.True.After(2000);
            Assert.That(() => true, constraint);
            _startCancelButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("PowerTube turned off")]
        [TestCase("Light is turned off")]
        [TestCase("Display cleared")]
        [Test]
        public void OpenDoor_WhileRunning_Test(string result)
        {
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            var constraint = Is.True.After(2000);
            Assert.That(() => true, constraint);
            _startCancelButton.Press();
            _door.Open();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_Just_PowerButtonPress_Test(string result)
        {
            _door.Open();
            _powerButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_Just_StartCancelButtonPress_Test(string result)
        {
            _door.Open();
            _startCancelButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_Just_TimeButtonButtonPress_Test(string result)
        {
            _door.Open();
            _timeButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_StartCancelAndPowerButtonPress_Test(string result)
        {
            _door.Open();
            _powerButton.Press();
            _startCancelButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_TimeAndPowerButtonPress_Test(string result)
        {
            _door.Open();
            _powerButton.Press();
            _timeButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_RunnigMicrowave_Test(string result)
        {
            _door.Open();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            var constraint = Is.True.After(62000);
            Assert.That(() => true, constraint);

            StringAssert.IsMatch(result, _stringWriter.ToString());
        }

        [TestCase("Light is turned on")]
        [Test]
        public void DoorOpen_RunnigMicrowaveAndCanceling_Test(string result)
        {
            _door.Open();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            var constraint = Is.True.After(2000);
            Assert.That(() => true, constraint);
            _startCancelButton.Press();
            StringAssert.IsMatch(result, _stringWriter.ToString());
        }
    }
}
