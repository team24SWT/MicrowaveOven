using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    class PowerButtonToUserInterface
    {
        private Output output_;
        private Display display_;
        private PowerTube PT_;
        private Light light_;
        private CookController CC_;
        private UserInterface UI_;
        private Timer timer_;
        private Button powerB_;
        private StringWriter SW_;

        private IDoor door_;
        private IButton timeB_;
        private IButton startCancelB_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            timer_ = new Timer();
            powerB_ = new Button();
            display_ = new Display(output_);
            PT_ = new PowerTube(output_);
            light_ = new Light(output_);
            CC_ = new CookController(timer_, display_, PT_);

            door_ = Substitute.For<IDoor>();
            timeB_ = Substitute.For<IButton>();
            startCancelB_ = Substitute.For<IButton>();

            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);
            CC_.UI = UI_;
            SW_ = new StringWriter();
            Console.SetOut(SW_);
        }

        [Test]
        public void PressPowerOnceShowUI()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Press();
            StringAssert.Contains("50 W", SW_.ToString());
        }

        [Test]
        public void PressPowerMultipleTimes()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            for (int i = 0; i < 8; i++)
            {
                powerB_.Press();
            }
            StringAssert.Contains("400 W", SW_.ToString());
        }

        [Test]
        public void PressPowerToMaxLimit()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            for (int i = 0; i < 14; i++)
            {
                powerB_.Press();
            }
            StringAssert.Contains("700 W", SW_.ToString());
        }

        [Test]
        public void PressPowerToOverLimit()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            for (int i = 0; i < 15; i++)
            {
                powerB_.Press();
            }
            StringAssert.DoesNotContain("750 W", SW_.ToString());
        }
    }
}
