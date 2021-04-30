using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;
using System.IO;

namespace Microwave.Test.Integration
{
    class UserInterfaceToDisplayTest
    {
        private Output output_;
        private Display display_;
        private UserInterface UI_;

        //--------------------

        private IDoor door_;
        private ILight light_;
        private ICookController CC_;
        private IButton powerB_;
        private IButton startCancelB_;
        private IButton timeB_;
        private StringWriter SW_;


        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            display_ = new Display(output_);

            CC_ = Substitute.For<ICookController>();
            startCancelB_ = Substitute.For<IButton>();
            powerB_ = Substitute.For<IButton>();
            timeB_ = Substitute.For<IButton>();
            light_ = Substitute.For<ILight>();
            door_ = Substitute.For<IDoor>();

            SW_ = new StringWriter();
            Console.SetOut(SW_);

            UI_ = new UserInterface(powerB_, timeB_, startCancelB_, door_, display_, light_, CC_);

        }

       [Test]
       public void TestNothingOnTheDisplay_TestForDoorOpenReady()
        {
            string str = "50";
            door_.Opened += Raise.Event();
            StringAssert.DoesNotContain(str, SW_.ToString());
        }


        [Test]
        public void TestDoorOpenSetPower_PowerClearDisplay()
        {
            string str = "Display Cleared";
            powerB_.Pressed += Raise.Event();
            StringWriter testSW_ = new StringWriter();
            Console.SetOut(testSW_);
            door_.Opened += Raise.Event();
            StringAssert.DoesNotContain(str, testSW_.ToString());

        }

        [Test]
        public void TestForDoorOpen_SetTimeClearDisplay()
        {
            string str = "Display Cleared";
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            StringWriter testSW_ = new StringWriter();
            Console.SetOut(testSW_);
            door_.Opened += Raise.Event();
            StringAssert.DoesNotContain(str, testSW_.ToString());
        }

        [Test]
        public void TestForDoorOpen_ClearCookingDisplay()
        {
            string str = "Display Cleared";
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            StringWriter testSW_ = new StringWriter();
            Console.SetOut(testSW_);
            door_.Opened += Raise.Event();
            StringAssert.DoesNotContain(str, testSW_.ToString());
        }

        [Test]
        public void TestPressedPowerButton_OnTheDisplay()
        {
            string str = "50";
            powerB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestFor3Pressed_PowerButtonOnDisplay()
        {
            string str = "150";
            for (int i = 0; i < 3; i++)
            {
                powerB_.Pressed += Raise.Event();
            }
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestFor14Pressed_PowerButtonOnDisplay()
        {
            string str = "700";
            for (int i = 0; i < 14; i++)
            {
                powerB_.Pressed += Raise.Event();
            }
               
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestFor15Pressed_PowerButtonOnDisplay()
        {
            string str = "750";
            for (int i = 0; i < 15; i++)
            {
                powerB_.Pressed += Raise.Event();
            }

            StringAssert.DoesNotContain(str, SW_.ToString());
        }

        [Test]
        public void TestTimeButtonPressed_OnTheDisplay()
        {
            string str = "1";
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestFor5Pressed_TimeButtonOnDisplay()
        {
            string str = "5";
            powerB_.Pressed += Raise.Event();
            for (int i = 0; i < 5; i++)
            {
                timeB_.Pressed += Raise.Event();
            }
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestFor60Pressed_TimeButtonOnDisplay()
        {
            string str = "60";
            powerB_.Pressed += Raise.Event();
            for (int i = 0; i < 60; i++)
            {
                timeB_.Pressed += Raise.Event();
            }
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestFor61Pressed_TimeButtonOnDisplay()
        {
            string str = "1";
            powerB_.Pressed += Raise.Event();
            StringWriter testSW_ = new StringWriter();
            for (int i = 0; i < 60; i++)
            {
                timeB_.Pressed += Raise.Event();
            }
            Console.SetOut(testSW_);
            timeB_.Pressed += Raise.Event();
            StringAssert.Contains(str, testSW_.ToString());
        }

        [Test]
        public void TestStartButton_PressedAfterPower()
        {
            string str = "Display cleared";
            powerB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestStartButton_PressedBefore()
        {
            string str = "Display cleared";
            powerB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestStartButton_PressedAfterCooking()
        {
            string str = "Display cleared";
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TestTimerExpired()
        {
            string str = "Display cleared";
            powerB_.Pressed += Raise.Event();
            timeB_.Pressed += Raise.Event();
            startCancelB_.Pressed += Raise.Event();
            UI_.CookingIsDone();
            StringAssert.Contains(str, SW_.ToString());
        }
    }
}
