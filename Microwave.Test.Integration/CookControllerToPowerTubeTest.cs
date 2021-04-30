using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    class CookControllerToPowerTubeTest
    {
        private Output output_;
        private PowerTube PT_;
        private Display display_;
        private CookController CC_;
        private StringWriter SW_;
        private ITimer timer_;
        private IUserInterface UI_;
     
        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            PT_ = new PowerTube(output_);
            display_ = new Display(output_);
            timer_ = Substitute.For<ITimer>();
            UI_ = Substitute.For<IUserInterface>();
            CC_ = new CookController(timer_, display_, PT_, UI_);
        }

        [TestCase(50)]
        [TestCase(250)]
        [TestCase(450)]
        [TestCase(700)]
        public void StartOwnsetTurnOn(int power)
        {
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            int time = 60;
            CC_.StartCooking(power, time);
            string str = ($"PowerTube works with {power}");
            StringAssert.Contains(str, SW_.ToString());
        }
        [Test]
        public void CalledTurnOff()
        {
            int time = 3600;
            int power = 200;
            string str = "PowerTube turned off";
            CC_.StartCooking(power, time);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            CC_.Stop();
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void CalledTurnOffTimerExpired()
        {
            int time = 3600;
            int power = 100;
            string str = "PowerTube turned off";
            CC_.StartCooking(power, time);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            timer_.Expired += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());
        }


    }
}
