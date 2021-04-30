using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    class CookControllerToUserInterfaceTest
    {
        private Output output_;
        private PowerTube PT_;
        private Display display_;
        private CookController CC_;
        private StringWriter SW_;
        private Timer timer_;
        private Light light_;
        private UserInterface UI_;

        private IDoor door_;
        private IButton powerB_;
        private IButton timeB_;
        private IButton startCancelB_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            timer_ = new Timer();
            PT_ = new PowerTube(output_);
            display_ = new Display(output_);
            light_ = new Light(output_);
            
            SW_ = new StringWriter();
            Console.SetOut(SW_);

            door_ = Substitute.For<IDoor>();
            powerB_ = Substitute.For<IButton>();
            timeB_ = Substitute.For<IButton>();
            startCancelB_ = Substitute.For<IButton>();

            CC_ = new CookController(timer_, display_, PT_);
            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);
            CC_.UI = UI_;
        }

        [Test]
        public void MinimalPowerPressedWithTimeAndStart()
        {

            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();

            StringAssert.Contains("PowerTube works with 50", SW_.ToString());
        }

        [Test]
        public void MaxPowerPressedWithTimeAndStart()
        {
            for (int i = 0; i < 14; i++)
            {
                powerB_.Pressed += Raise.Event();
            }
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();

            StringAssert.Contains("PowerTube works with 700", SW_.ToString());
        }

        [Test]
        public void MinimalPowerPressedWithTimeAndStartThenStop()
        {

            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();

            StringAssert.Contains("PowerTube turned off", SW_.ToString());
        }

        [Test]
        public void MinimalPowerPressedWithTimeAndStartThenOpenDoor()
        {

            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            door_.Opened += Raise.Event();

            StringAssert.Contains("PowerTube turned off", SW_.ToString());
        }
    }
}
