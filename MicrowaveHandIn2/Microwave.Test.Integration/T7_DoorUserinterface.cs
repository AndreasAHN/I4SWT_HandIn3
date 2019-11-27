using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T7_DoorUserinterface
    {
        private IDoor _sut;
        private IUserInterface _userInterface;
        private CookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private IButton _powerButton, _timeButton, _startCancelButton; // fake
        private ITimer _timer;           // fake
        private IPowerTube _powerTube;   // fake
        private IOutput _output;         // fake



            [SetUp]
        public void Setup()
        {
            _sut = new Door(); 
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _output = Substitute.For<IOutput>();

            _light = new Light(_output);
            _display = new Display(_output);
            
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _sut, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void DoorOpenedWhileIdle()
        {
            _userInterface.OnDoorOpened(this, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void DoorOpenedThenClosedWhileIdle()
        {
            _userInterface.OnDoorOpened(this, EventArgs.Empty);
            _userInterface.OnDoorClosed(this, EventArgs.Empty);

            Received.InOrder(() =>
            {
                _output.OutputLine("Light is turned on");
                _output.OutputLine("Light is turned off");
                _output.OutputLine("Display cleared");
            });

        }

        [Test]
        public void DoorOpenedDuringSetup()
        {
            _powerButton.Press();
            _timeButton.Press();
            _userInterface.OnDoorOpened(this, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void DoorOpenedThenClosedDuringSetup()
        {
            _powerButton.Press();
            _timeButton.Press();
            _userInterface.OnDoorOpened(this, EventArgs.Empty);
            _userInterface.OnDoorClosed(this, EventArgs.Empty);

            Received.InOrder(() =>
            {
                _output.OutputLine("Light is turned on");
                _output.OutputLine("Light is turned off");
            });
        }

        [Test]
        public void DoorOpenedWhileRunning()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _userInterface.OnDoorOpened(this, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned on");


        }


    }
}
