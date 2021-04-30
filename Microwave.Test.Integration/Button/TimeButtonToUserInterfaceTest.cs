using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;


namespace Microwave.Test.Integration
{
    class TimeButtonToUserInterfaceTest
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
        private StringWriter SW_;

        private IDoor door_;
        private IButton startCancelB_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            timer_ = new Timer();
            powerB_ = new Button();
            timeB_ = new Button();
            display_ = new Display(output_);
            PT_ = new PowerTube(output_);
            light_ = new Light(output_);
            CC_ = new CookController(timer_, display_, PT_);

            door_ = Substitute.For<IDoor>();
            startCancelB_ = Substitute.For<IButton>();

            SW_ = new StringWriter();
            Console.SetOut(SW_);

            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);
            CC_.UI = UI_; 
        }

        [Test]
        public void PressPowerThenTimeOnceShowUI()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Press();
            timeB_.Press();
            StringAssert.Contains("01:00", SW_.ToString());
        }

        [Test]
        public void PressPowerThenTimeMultipleShowUI()
        {
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Press();
            for (int i = 0; i < 50; i++)
            {
                timeB_.Press();
            }
            StringAssert.Contains("50:00", SW_.ToString());
        }

    }
}
