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
using MicrowaveOvenClasses.Controllers;


namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T6_UserInterface
    {
        private IOutput _output;

        private ILight _light;
        private IUserInterface _sut;
        private IDoor _door;
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private CookController _cookController;

        [SetUp]
        public void Setup()
        {
            // Fakes
            _output = Substitute.For<IOutput>();
            _door = Substitute.For<IDoor>();
            _startCancelButton = Substitute.For<IButton>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _timer = Substitute.For<ITimer>();

            // Real - include
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            // 
            _sut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _sut;
        }

        #region OnDoorOpen

        [Test]
        public void OnDoorOpen_StateReady_LightTurnOn()
        {
            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void OnDoorOpen_StatePower_LightTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void OnDoorOpen_StateSetTime_LightTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void OnDoorOpen_StateCooking_CookingStop_PowerTubeTurnOff()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }

        [Test]
        public void OnDoorOpen_StateCooking_CookingStop_TimerStop()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _door.Opened += Raise.Event();

            _timer.Received().Stop();
        }


        #endregion

        #region OnDoorClose

        [Test]
        public void OnDoorClose_LightTurnOff()
        {
            _door.Opened += Raise.Event();
            _door.Closed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }
        #endregion

        #region OnTimePressed

        [Test]
        public void OnTime_StateSetTime_TimeButtonPressedOnce_DisplayShowsTime()
        {
            _powerButton.Pressed += Raise.Event();

            _timeButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains($"Display shows: 01:00")));
        }

        [Test]
        public void OnTime_StateSetTime_TimeButtonPressedTwice_DisplayShowsTime()
        {
            _powerButton.Pressed += Raise.Event();

            _timeButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(x => x.Contains($"Display shows: 02:00")));
        }


        #endregion

        #region OnPowerPressed

        [Test]
        public void OnPowerPressed_StateReady_DisplayShowsPower()
        {
            _powerButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 50 W")));
        }

        [Test]
        public void OnPowerPressedTwice_StateReady_DisplayShowsPower()
        {
            _powerButton.Pressed += Raise.Event();
            _powerButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 100 W")));
        }

        #endregion

        #region OnStartCancelPressed

        [Test]
        public void OnStartCancel_StateReady_NothingHappens()
        {
            _startCancelButton.Pressed += Raise.Event();

            _output.Received(0);
        }

        [Test]
        public void OnStartCancel_StateSetPower_NothingHappens()
        {
            _powerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _output.Received(0);
        }

        public void OnStartCancel_StateSetPower_DisplayCleared()
        {
            _powerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }

        [Test]
        public void OnStartCancel_StateSetTime_LightsTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void OnStartCancel_StateSetTime_DisplayCleared()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }

        [Test]
        public void OnStartCancel_StateSetTime_StartCooking_PowerTubeTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube works with 50 %")));
        }

        [Test]
        public void OnStartCancel_StateSetTime_StartCooking_StartTime()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event(); // Time = 60

            _startCancelButton.Pressed += Raise.Event();

            _timer.Received().Start(60);
        }

        [Test]
        public void OnStartCancel_StateCooking_StopCooking_PowerTubeTurnOff()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }

        [Test]
        public void OnStartCancel_StateCooking_StopCooking_TimeStop()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _timer.Received().Stop();
        }

        [Test]
        public void OnStartCancel_StateCooking_DisplayCleared()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event(); // Display cleared

            _startCancelButton.Pressed += Raise.Event(); // Display cleared

            _output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }

        [Test]
        public void OnStartCancel_StateCooking_LightsTurnOff()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }





        #endregion

        #region CookingIsDone

        [Test]
        public void CookingIsDone_LightTurnOff()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _timer.Expired += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }

        [Test]
        public void CookingIsDone_DisplayClear()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();
            // Display clear


            _timer.Expired += Raise.Event();


            _output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }

        #endregion
    }
}
