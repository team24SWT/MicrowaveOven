using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    class StartCancelButtonToUserInterfaceTest
    {
        private Output output_;
        private Display display_;
        private PowerTube PT_;
        private Light light_;
        private CookController CC_;
        private UserInterface UI_;
        private Timer timer_;
        private Button powerB_;
        private Button timeB_;
        private Button startCancelB_;
        private StringWriter SW_;

        private IDoor door_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            timer_ = new Timer();
            powerB_ = new Button();
            timeB_ = new Button();
            startCancelB_ = new Button();
            display_ = new Display(output_);
            PT_ = new PowerTube(output_);
            light_ = new Light(output_);
            CC_ = new CookController(timer_, display_, PT_);

            door_ = Substitute.For<IDoor>();
      
            SW_ = new StringWriter();
            Console.SetOut(SW_);

            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);
            CC_.UI = UI_;
        }

        [Test]
        public void CancelFromSetPowerStateShowUI()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Press();
            startCancelB_.Press();
            StringAssert.Contains("Display cleared", SW_.ToString());
        }

        [Test]
        public void StartFromSetTimeStateShowUI()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Press();
            timeB_.Press();
            startCancelB_.Press();
            StringAssert.Contains("50 W", SW_.ToString());
            StringAssert.Contains("Light is turned on", SW_.ToString());
            StringAssert.Contains("PowerTube works with 50", SW_.ToString());

            StringAssert.DoesNotContain("Display cleared", SW_.ToString());
        }

        [Test]
        public void CancelFromCookingStateShowUI()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Press();
            timeB_.Press();
            startCancelB_.Press();
            startCancelB_.Press();

            StringAssert.Contains("50 W", SW_.ToString());
            StringAssert.Contains("Light is turned on", SW_.ToString());
            StringAssert.Contains("PowerTube works with 50", SW_.ToString());

            StringAssert.Contains("Display cleared", SW_.ToString());
        }
    }
}
