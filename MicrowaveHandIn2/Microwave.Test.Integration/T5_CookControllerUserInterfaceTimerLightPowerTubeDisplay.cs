﻿using System;
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
    public class T5_CookControllerUserInterfaceTimerLightPowerTubeDisplay
    {
        private IOutput _output;
        
        private ILight _light;
        private IUserInterface _sut;
        private IDoor _door;
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;
        private IDisplay _display;
        private ICookController _cookController;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);

            _door = Substitute.For<IDoor>();
            _startCancelButton = Substitute.For<IButton>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _display = Substitute.For<IDisplay>();
            _cookController = Substitute.For<ICookController>();

            _sut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        #region OnDoorOpen

        [Test]
        public void OnDoorOpen_StateReady_LightTurnOn()
        {
            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorOpen_StatePower_LightTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorOpen_StateSetTime_LightTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _door.Opened += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorOpen_StateCooking_LightTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _output.Received(0).OutputLine(Arg.Any<string>());
        }


        #endregion

        #region OnDoorClose

        [Test]
        public void OnDoorClose_LightTurnOff()
        {
            _door.Opened += Raise.Event();
            _door.Closed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
        #endregion

        #region OnStartCancel

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

        [Test]
        public void OnStartCancel_StateTime_LightsTurnOn()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnStartCancel_StateCooking_LightsTurnOff()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _startCancelButton.Pressed += Raise.Event();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        #endregion

        #region CookingIsDone

        [Test]
        public void CookingIsDone_LightTurnOff()
        {
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _sut.CookingIsDone();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        #endregion
    }
}
