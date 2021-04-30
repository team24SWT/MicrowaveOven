using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace Microwave.Test.Integration.CookControllers
{
    class TimerToCookControlerTest
    {
        private Output output_;
        private PowerTube PT_;
        private Display display_;
        private CookController CC_;
        private StringWriter SW_;
        private Classes.Boundary.Timer timer_;
        private IUserInterface UI_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            PT_ = new PowerTube(output_);
            display_ = new Display(output_);
            timer_ = new Classes.Boundary.Timer();
            UI_ = Substitute.For<IUserInterface>();
            CC_ = new CookController(timer_, display_, PT_);
            //CC_ = new CookController(timer_, display_, PT_, UI_);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
        }

        [Test]
        public void OnTimerTick()
        {
            int power = 300;
            int time = 6000;
            int timert = 1200;
            CC_.StartCooking(power, time);
            Thread.Sleep(timert);
            string str = $"Display shows: {(5999)/ 60:D2}:{(5999) % 60:D2}";
            StringAssert.Contains(str, SW_.ToString());
        }
        [Test]

        public void OnTimerExpiredTurnOff()
        {
            int time = 6;
            int power = 300;
            int timert = 6200;
            string str = "PowerTube turned off";
            CC_.StartCooking(power, time);
            Thread.Sleep(timert);
            StringAssert.Contains(str, SW_.ToString());
        }


    }
}
