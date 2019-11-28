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
using System.IO;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class T5_CookControllerUserInterfaceTimerLightPowerTubeDisplay
    {
        private ICookController _sut;


        private IOutput _output;
        private IPowerTube _powertube;
        private IDisplay _display;
        private ITimer _timer;
        private ILight _light;
        private IUserInterface _userinterface;

        //private StringWriter _stringWriter;





        [SetUp]
        public void SetUp()
        {
            //subs
            _output = Substitute.For<IOutput>();
            _userinterface = Substitute.For<IUserInterface>();


            //real
            _powertube = new PowerTube(_output);
            _display = new Display(_output);
            _timer = new Timer();
            _light = new Light(_output);

           _sut = new CookController(_timer, _display, _powertube);

        }


        //_________________________________________________________________________________________
        //StartCooking()
        [TestCase(0)]
        [TestCase(101)]
        public void On_StartCooking_Show_Power_Exception(int power)
        {
          
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.StartCooking(power,5));

        }

        

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(99)]
        [TestCase(100)]
        public void On_StartCooking_Show_Power(int power)
        {
            _sut.StartCooking(power, 5);

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube works with {power} %")));

        }



        [Test]
        public void On_StartCooking_Is_On_Exception()
        {
            _sut.StartCooking(89, 1);
            Assert.Throws<ApplicationException>(() => _sut.StartCooking(80, 1));

        }



        [Test]
        public void On_StartCooking_Show_isCooking()
        {
            _sut.StartCooking(80, 5);

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube works with 80 %")));

        }







        //_________________________________________________________________________________
        //Stop()
        [Test]
        public void on_Stop_Show_isCooking_true()
        {
            _timer.Start(2);

            var constraint = Is.True.After(1000);

            Assert.That(() => true, constraint);

            _output.Received(1);

        }


        [Test]  
        public void on_Stop_Show_Power_off()
        {
            _sut.StartCooking(80, 1);

            var constraint = Is.True.After(1000);

            _sut.Stop();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));

        }


        

        //__________________________________________________________________________________________
        // OnTimerExpired()
        [Test]
        public void OnTimerExpired_Powertube_off()
        {
            
            _sut.StartCooking(80, 1);
            var constraint = Is.True.After(1100);

            Assert.That(() => true, constraint); 


            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));

        }


        [Test]
        public void OnTimerExpired_Cooking_done()
        {
            _timer.Start(2);

            var constraint = Is.True.After(2100);

            Assert.That(() => true, constraint); //wait

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 00:00")));

        }


        [Test]
        public void OnTimerExpired_isCooking_false_NothingHappens()
        {
            _timer.Start(2);

            var constraint = Is.True.After(2100);

            Assert.That(() => true, constraint);

            _output.Received(0);
        }






        //___________________________________________________________________________________
        //OnTimerTick()
        [Test]
        public void OnTimerTick_showTime()
        {
            _sut.StartCooking(80, 2);

            var constraint = Is.True.After(1100);
            Assert.That(() => true, constraint); // force it to wait 1100 ms

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 00:01")));


        }







    }
}




