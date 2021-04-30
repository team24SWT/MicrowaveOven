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
    class CookControlerToTimer
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
            CC_ = new CookController(timer_, display_, PT_, UI_);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
        }

        [Test]
        
        public void StartCookCalledStartTimer()
        {
            int time = 2000;
            int power = 300;
            CC_.StartCooking(power, time);
            Assert.That(timer_.TimeRemaining, Is.EqualTo(time));
        }

       [Test]
        public void StopCookCalledStopTimer()
        {
            int power = 300;
            int time = 6000;
            int timert = 1200;
            CC_.StartCooking(power, time);
            Thread.Sleep(timert);
            CC_.Stop();
            Thread.Sleep(timert);
            int str = time - (timert / 1000);
            Assert.That(timer_.TimeRemaining, Is.EqualTo(str));
        }

    }
}
