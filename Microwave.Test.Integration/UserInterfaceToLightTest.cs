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
    class UserInterfaceToLightTest
    {
        private Output output_;
        private Display display_;       
        private StringWriter SW_;
        private Light light_;
        private UserInterface UI_;

        private ICookController CC_;
        public IDoor door_;
        public IButton powerB_;
        public IButton timeB_;
        public IButton startCancelB_;



        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            display_ = new Display(output_);
            light_ = new Light(output_);

            CC_ = Substitute.For<ICookController>();
            door_ = Substitute.For<IDoor>();
            powerB_ = Substitute.For<IButton>();
            timeB_ = Substitute.For<IButton>();
            startCancelB_ = Substitute.For<IButton>();
            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);

        }

        [Test]
        public void READYToTurnOnOnDoorOppened()
        {
            string str = "Light is turned on";
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            door_.Opened += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());

        }

        [Test]
        public void OnDoorClosedDOOROPENTurnOff()
        {
            string str = "Light is turned off";
            door_.Opened += Raise.Event();
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            door_.Closed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());

        }
        [Test]
        public void OnDoorOppenedSETPOWERTurnOn()
        {
            string str = "Light is turned on";
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Pressed += Raise.Event();
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            door_.Opened += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());

        }

        [Test]
        public void OnDoorOppenedSETTIMETurnOn()
        {
            string str = "Light is turned on";
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            door_.Opened += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());

        }
        [Test]
        public void OnStartCancelPressedSETTIMETurnOn()
        {
            string str = "Light is turned on";
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            startCancelB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());

        }

        [Test]
        public void OnStartCancelPressedSETPOWERTurnOff()
        {
            string str = "Light is turned on";
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Pressed += Raise.Event();           
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            startCancelB_.Pressed += Raise.Event();
            StringAssert.DoesNotContain(str, SW_.ToString());

        }

        [Test]
        public void OnStartCancelPressedCOOKINGTurnOff()
        {
            string str = "Light is turned off";
            door_.Opened += Raise.Event();
            door_.Closed += Raise.Event();
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            startCancelB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());

        }

        [Test]
        public void CookingIsDoneCOOKINGTurnOff()
        {
            string str = "Light is turned off";
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            SW_ = new StringWriter();
            Console.SetOut(SW_);
            UI_.CookingIsDone();
            StringAssert.Contains(str, SW_.ToString());

        }

    }
}
