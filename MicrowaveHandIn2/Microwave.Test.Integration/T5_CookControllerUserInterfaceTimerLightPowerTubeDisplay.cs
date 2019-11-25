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
    public class T5_CookControllerUserInterfaceTimerLightPowerTubeDisplay
    {
        private Output _output;
        private Display _display;
        private PowerTube _powerTube;
        private Timer _timer;
        private IUserInterface _fakeUserInterface;

        private CookController _uut;


        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _timer = new Timer();
            _fakeUserInterface = Substitute.For<IUserInterface>();

            _uut = new CookController(_timer, _display, _powerTube, _fakeUserInterface);
        }

        //[TestCase(1)]
        //[TestCase(2)]
        //[TestCase(99)]
        //[TestCase(100)]
        //public void StartCooking_NoExceptionThrown(int arg)
        //{
        //    Assert.That(() => _uut.StartCooking(arg, 60), Throws.Nothing);
        //}

        //[TestCase(-1)]
        //[TestCase(0)]
        //[TestCase(101)]
        //public void StartCooking_ExceptionThrown(int arg)
        //{
        //    Assert.That(() => _uut.StartCooking(arg, 60), Throws.Exception);
        //}


    }
}
