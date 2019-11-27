using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute.Core.DependencyInjection;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T4T5_TimerCookController
    {
        private ICookController _cookController;
        private ITimer _sut;

        // MOQ
        private IDisplay _display;
        private IPowerTube _powerTube;

        // FAKE
        private IUserInterface _userInterface;



        
        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();
            _userInterface = Substitute.For<IUserInterface>();

            _sut = new Timer();
            _cookController = new CookController(_sut, _display, _powerTube, _userInterface);
        }

        [Test]
        public void StartCookingTwoSeconds_TestTimeLeftIsRightBeforeExpired()
        {
            _cookController.StartCooking(500, 2);
            
            // Wait for time (in sec) - 100 ms to test just before time is expired
            var constraint = Is.True.After(1900); // ms
            

            Assert.That(() => _sut.TimeRemaining == 1, constraint);
        }

        [Test]
        public void StartCookingTwoSeconds_TestTimeLeftIsRightAfterExpired()
        {
            _cookController.StartCooking(500, 2);

            // Wait for time (in sec) - 100 ms to test just before time is expired
            var constraint = Is.True.After(2100); // ms


            Assert.That(() => _sut.TimeRemaining == 0, constraint);
        }

        [Test]
        public void StartCookingTwoSeconds_StopCookingTimerStop()
        {
            _cookController.StartCooking(500, 2);

            // Wait 1.1 second
            var constraint = Is.True.After(1100); // ms
            Assert.That(() => true, constraint); // Just to make it wait, not a real Assert

            _cookController.Stop();
            // wait another second
            constraint = Is.True.After(1000); // ms

            Assert.That(() => _sut.TimeRemaining == 1, constraint);
        }

        [Test]
        public void StartCookingOneSecond_TimeExpireEventRaised()
        {
            _cookController.StartCooking(500, 1);

            var constraint = Is.True.After(1100);

            Assert.That(() => true, constraint); // Just to make it wait, not a real Assert

            _powerTube.Received(1).TurnOff();
        }

        [Test]
        public void StartCookingTwoSeconds_DisplayShowsNewTimeTwice()
        {
            _cookController.StartCooking(500, 2);

            var constraint = Is.True.After(2100);
            Assert.That(() => true, constraint); // Just to make it wait, not a real Assert

            _display.Received(1).ShowTime(0, 1);
            _display.Received(1).ShowTime(0, 0);
        }
    }
}
