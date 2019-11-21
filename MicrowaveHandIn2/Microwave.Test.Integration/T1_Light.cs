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
    public class T1_Light
    {
        private ILight _light;
        private Output _output;



        [SetUp]
        public void Setup()
        {
            _light = new Light();
            _output = new Output();
        }




        //much alike unit test
        [Test]
        public void TurnOn_WasOff_CorrectOutput()
        {
            _light.TurnOn();
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
            
        }


        [Test]
        public void TurnOff_WasOn_CorrectOutput()
        {
            _light.TurnOn();
            _light.TurnOff();
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }


        [Test]
        public void TurnOn_WasOn_CorrectOutput()
        {
            _light.TurnOn();
            _light.TurnOn();
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void TurnOff_WasOff_CorrectOutput()
        {
            _light.TurnOff();
            output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }
}
