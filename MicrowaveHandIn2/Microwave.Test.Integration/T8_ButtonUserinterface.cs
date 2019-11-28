using System;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T8_ButtonUserinterface
    {
        private IButton _powerButton, _timeButton, _startCancelButton;
        private IUserInterface _userInterface;
        private CookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private IDoor _door;
        private ITimer _timer; 
        private IPowerTube _powerTube;
        private IOutput _output;


        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = Substitute.For<IDoor>(); 
            _timer = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = Substitute.For<IPowerTube>();



            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void PowerButtonPressedOnce_Shows50W()
        {
            _userInterface.OnPowerPressed(this,EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void PowerButtonPressedTwice_Shows100W()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnPowerPressed(this, EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 100 W");
        }

        [TestCase(1, 50)]
        [TestCase(2, 100)]
        [TestCase(10, 500)]
        [TestCase(14, 700)]
        public void PowerButtonPressedNtimes(int nPresses, int expected)
        {
            for (int i = 0; i < nPresses; i++)
            {
                _userInterface.OnPowerPressed(this, EventArgs.Empty);
            }
            _output.Received(1).OutputLine($"Display shows: {expected} W");
        }

        [Test]
        public void TimeButtonPressedOnce_Shows0100()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnTimePressed(this, EventArgs.Empty);
            
            _output.Received(1).OutputLine("Display shows: 01:00");
        }

        [Test]
        public void TimeButtonPressedTwice_Shows0200()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnTimePressed(this, EventArgs.Empty);
            _userInterface.OnTimePressed(this, EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 02:00");
        }

        [TestCase(1, "01:00")]
        [TestCase(2, "02:00")]
        [TestCase(6, "06:00")]
        [TestCase(11, "11:00")]
        public void TimeButtonPressedNtimes(int nPresses, string expected)
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            for (int i = 0; i < nPresses; i++)
            {
                _userInterface.OnTimePressed(this, EventArgs.Empty);
            }
            _output.Received(1).OutputLine($"Display shows: {expected}");
        }

        [Test]
        public void StartCancelBtnPressedAfterPowerBtn()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);

            Received.InOrder(() =>
                {
                    _output.OutputLine("Display shows: 50 W");
                    _output.OutputLine("Display cleared");
                });
        }

        [Test]
        public void StartCancelBtnPressedAfterPowerBtnTwice()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnPowerPressed(this,EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);
            Received.InOrder(() =>
            {
                _output.OutputLine("Display shows: 50 W");
                _output.OutputLine("Display shows: 100 W");
                _output.OutputLine("Display cleared");
            });
        }

        [Test]
        public void StartCancelBtnPressedTwiceAfterPowerBtn()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);

            Received.InOrder(() =>
            {
                _output.OutputLine("Display shows: 50 W");
                _output.OutputLine("Display cleared");
            });
        }

        [Test]
        public void StartCancelBtnPressedAfterPowerAndTimeBtn()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnTimePressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);
            Received.InOrder(() =>
            {
                _output.OutputLine("Display shows: 50 W");
                _output.OutputLine("Display shows: 01:00");
                _output.OutputLine("Display cleared");
                _output.OutputLine("Light is turned on");
            });
        }

        [Test]
        public void StartCancelBtnPressedTwiceAfterPowerAndTimeBtnTwice()
        {
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnPowerPressed(this, EventArgs.Empty);
            _userInterface.OnTimePressed(this, EventArgs.Empty);
            _userInterface.OnTimePressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);
            Received.InOrder(() =>
            {
                _output.OutputLine("Display shows: 50 W");
                _output.OutputLine("Display shows: 100 W");
                _output.OutputLine("Display shows: 01:00");
                _output.OutputLine("Display shows: 02:00");
                _output.OutputLine("Display cleared");
                _output.OutputLine("Light is turned on");
                _output.OutputLine("Light is turned off");
                _output.OutputLine("Display cleared");
            });
        }

        [Test]
        public void StartCancelBtnPressedFirstOnce_NothingHappens()
        {
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received(0);
        }

        [Test]
        public void StartCancelBtnPressedFirstTwice_NothingHappens()
        {
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received(0);
        }
    }
}
