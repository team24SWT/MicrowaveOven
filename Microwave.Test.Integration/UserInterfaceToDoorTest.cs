using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace Microwave.Test.Integration
{
    class UserInterfaceToDoorTest
    {
        private Output output_;
        private Display display_;
        private PowerTube PT_;
        private Light light_;
        private CookController CC_;
        private UserInterface UI_;
        private Classes.Boundary.Timer timer_;
        private Button powerB_;
        private Button timeB_;
        private Button startCancelB_;
        private StringWriter SW_;
        private Door door_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            timer_ = new Classes.Boundary.Timer();
            powerB_ = new Button();
            timeB_ = new Button();
            startCancelB_ = new Button();
            display_ = new Display(output_);
            PT_ = new PowerTube(output_);
            light_ = new Light(output_);
            CC_ = new CookController(timer_, display_, PT_);
            door_ = new Door();

            SW_ = new StringWriter();
            Console.SetOut(SW_);

            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);
            CC_.UI = UI_;
        }

        [Test]
        public void DoorOpenedShowUI()
        {
            door_.Open();
            StringAssert.Contains("Light is turned on", SW_.ToString());
        }

        [Test]
        public void DoorOpenedThenClosedShowUI()
        {
            door_.Open();
            door_.Close();
            StringAssert.Contains("Light is turned off", SW_.ToString());
        }

        [Test]
        public void SetPowerStateThenDoorOpenedShowUI()
        {
            door_.Open();
            door_.Close();
            powerB_.Press();
            door_.Open();
            StringAssert.Contains("Display cleared", SW_.ToString());
        }

        [Test]
        public void SetTimeStateThenDoorOpenedShowUI()
        {
            door_.Open();
            door_.Close();
            powerB_.Press();
            timeB_.Press();
            door_.Open();
            StringAssert.Contains("Display cleared", SW_.ToString());
        }

        [Test]
        public void CookingStateThenDoorOpenedShowUI()
        {
            door_.Open();
            door_.Close();
            powerB_.Press();
            timeB_.Press();
            startCancelB_.Press();
            door_.Open();
            StringAssert.Contains("Display cleared", SW_.ToString());
        }

        [Test]
        public void CookingIsDoneStateThenDoorOpenedShowUI()
        {
            StringWriter testSW_ = new StringWriter();

            door_.Open();
            door_.Close();
            powerB_.Press();
            timeB_.Press();
            startCancelB_.Press();
            Thread.Sleep(60200);
            Console.SetOut(testSW_);
            door_.Open();
            StringAssert.Contains("Light is turned on", testSW_.ToString());
        }

        [Test]
        public void CookingIsDoneStateThenDoorOpenedThenClosedShowUI()
        {
            StringWriter testSW_ = new StringWriter();

            door_.Open();
            door_.Close();
            powerB_.Press();
            timeB_.Press();
            startCancelB_.Press();
            Thread.Sleep(60200);
            Console.SetOut(testSW_);
            door_.Open();
            door_.Close();
            StringAssert.Contains("Light is turned off", testSW_.ToString());
        }
    }
}
