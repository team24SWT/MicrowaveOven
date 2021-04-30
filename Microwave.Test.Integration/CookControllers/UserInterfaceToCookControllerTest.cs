using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace Microwave.Test.Integration
{
    class UserInterfaceToCookControllerTest
    {
        private Output output_;
        private PowerTube PT_;
        private Display display_;
        private CookController CC_;
        private StringWriter SW_;
        private Classes.Boundary.Timer timer_;
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
            timer_ = new Classes.Boundary.Timer();
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
        public void UICookingIsDoneNotCalledWithoutEvents()
        {
            int power = 700;
            int time = 3;
            int timert = 3200;

            CC_.StartCooking(power, time);
            Thread.Sleep(timert);

            StringAssert.DoesNotContain("Display cleared", SW_.ToString());
        }

        [Test]
        public void UICookingIsDoneCalledWithEvents()
        {
            int timert = 60200;

            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            Thread.Sleep(timert);

            StringAssert.Contains("Display cleared", SW_.ToString());
        }
    }
}
