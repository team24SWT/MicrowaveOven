using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    class CookControllerToDisplayTest
    {
        private Output output_;
        private PowerTube PT_;
        private Display display_;
        private CookController CC_;
        private System.IO.StringWriter SW_;

        private ITimer timer_;
        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            PT_ = new PowerTube(output_);
            display_ = new Display(output_);
            timer_ = Substitute.For<ITimer>();
            CC_ = new CookController(timer_, display_, PT_);
        }

        [Test]
        public void OnTimerTick_called_myDisplay()
        {
            SW_ = new StringWriter();
            Console.SetOut(SW_);

            string str = "Display shows: 06:30";
            int power = 50;
            int time = 390;

            CC_.StartCooking(power, time);
            timer_.TimeRemaining.Returns(390);
            timer_.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            StringAssert.Contains(str, SW_.ToString());
        }
    }
}
