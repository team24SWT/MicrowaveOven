using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;


namespace Microwave.Test.Integration
{
    class UserinterfaceToDisplay
    {
        private Output output_;
        private PowerTube PT_;
        private Display display_;
        private UserInterface ui;

        //--------------------

        private IDoor door;
        private ILight light;
        private ICookController cookController;
        private IButton PowerBt;
        private IButton cancelBt;
        private IButton TimeBt;
        private System.IO.StringWriter stringWriter;


        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            PT_ = new PowerTube(output_);
            display_ = new Display(output_);

            cookController = Substitute.For<ICookController>();
            cancelBt = Substitute.For<IButton>();
            PowerBt = Substitute.For<IButton>();
            TimeBt = Substitute.For<IButton>();
            light = Substitute.For<ILight>();
            door = Substitute.For<IDoor>();

            stringWriter = new System.IO.StringWriter();
            Console.SetOut(stringWriter);

            ui = new UserInterface(PowerBt, TimeBt, cancelBt, door, display_, light, cookController);

        }

       [Test]
       public void TestNothingOnTheDisplay_TestForDoorOpenReady()
        {
            string expectedNot = "50";
            door.Opened += Raise.Event();
            StringAssert.DoesNotContain(expectedNot, stringWriter.ToString());

        }


        [Test]
        public void TestDoorOpenSetPower_PowerClearDisplay()
        {
            string expectedNot = "Display Cleared";
            PowerBt.Pressed += Raise.Event();
            System.IO.StringWriter stringWriter1 = new System.IO.StringWriter();
            Console.SetOut(stringWriter1);
            door.Opened += Raise.Event();
            StringAssert.DoesNotContain(expectedNot, stringWriter1.ToString());

        }

        [Test]
        public void TestForDoorOpen_SetTimeClearDisplay()
        {
            string expectedNot = "Display Cleared";
            PowerBt.Pressed += Raise.Event();
            TimeBt.Pressed += Raise.Event();
            System.IO.StringWriter stringWriter1 = new System.IO.StringWriter();
            Console.SetOut(stringWriter1);
            door.Opened += Raise.Event();
            StringAssert.DoesNotContain(expectedNot, stringWriter1.ToString());
        }

        [Test]
        public void TestForDoorOpen_ClearCookingDisplay()
        {
            string expectedNot = "Display Cleared";
            PowerBt.Pressed += Raise.Event();
            TimeBt.Pressed += Raise.Event();
            cancelBt.Pressed += Raise.Event();
            System.IO.StringWriter stringWriter1 = new System.IO.StringWriter();
            Console.SetOut(stringWriter1);
            door.Opened += Raise.Event();
            StringAssert.DoesNotContain(expectedNot, stringWriter1.ToString());
        }

        [Test]
        public void TestPressedPowerButton_OnTheDisplay()
        {
            string expectedNot = "51";
            PowerBt.Pressed += Raise.Event();
            StringAssert.DoesNotContain(expectedNot, stringWriter.ToString());
        }

        [Test]
        public void TestFor3Pressed_PowerButtonOnDisplay()
        {
            string expectedNot = "151";
            for (int i = 0; i < 3; i++)
            {
                PowerBt.Pressed += Raise.Event();
            }
            StringAssert.DoesNotContain(expectedNot, stringWriter.ToString());
        }

        [Test]
        public void TestFor14Pressed_PowerButtonOnDisplay()
        {
            string expectedNot = "750";
            for (int i = 0; i < 14; i++)
            {
                PowerBt.Pressed += Raise.Event();
            }
               
            StringAssert.DoesNotContain(expectedNot, stringWriter.ToString());
        }

        [Test]
        public void TestFor15Pressed_PowerButtonOnDisplay()
        {
            string expectedNot = "51";
            for (int i = 0; i < 15; i++)
            {
                PowerBt.Pressed += Raise.Event();
            }

            StringAssert.DoesNotContain(expectedNot, stringWriter.ToString());
        }

        [Test]
        public void TestTimeButtonPressed_OnTheDisplay()
        {
            string expectedNot = "2";
            PowerBt.Pressed += Raise.Event();
            TimeBt.Pressed += Raise.Event();
            StringAssert.DoesNotContain(expectedNot, stringWriter.ToString());
        }

        [Test]
        public void TestFor5Pressed_TimeButtonOnDisplay()
        {
            string expected = "5";
            PowerBt.Pressed += Raise.Event();
            for (int i = 0; i < 5; i++)
            {
                TimeBt.Pressed += Raise.Event();
            }
            StringAssert.Contains(expected, stringWriter.ToString());
        }

        [Test]
        public void TestFor60Pressed_TimeButtonOnDisplay()
        {
            string expected = "60";
            PowerBt.Pressed += Raise.Event();
            for (int i = 0; i < 60; i++)
            {
                TimeBt.Pressed += Raise.Event();
            }
            StringAssert.Contains(expected, stringWriter.ToString());
        }

        [Test]
        public void TestFor61Pressed_TimeButtonOnDisplay()
        {
            string expected = "60";
            PowerBt.Pressed += Raise.Event();
            PowerBt.Pressed += Raise.Event();
            System.IO.StringWriter stringWriter1 = new System.IO.StringWriter();
            Console.SetOut(stringWriter1);
            for (int i = 0; i < 61; i++)
            {
                TimeBt.Pressed += Raise.Event();
            }
            StringAssert.Contains(expected, stringWriter1.ToString());
        }

        [Test]
        public void TestStartButton_PressedAfterPower()
        {
            string expected = "Display cleared";
            PowerBt.Pressed += Raise.Event();
            cancelBt.Pressed += Raise.Event();
            StringAssert.Contains(expected, stringWriter.ToString());
        }

        [Test]
        public void TestStartButton_PressedBefore()
        {
            string expected = "Display cleared";
            PowerBt.Pressed += Raise.Event();
            cancelBt.Pressed += Raise.Event();
            StringAssert.Contains(expected, stringWriter.ToString());
        }

        [Test]
        public void TestStartButton_PressedAfterCooking()
        {
            string expected = "Display cleared";
            PowerBt.Pressed += Raise.Event();
            TimeBt.Pressed += Raise.Event();
            cancelBt.Pressed += Raise.Event();
            cancelBt.Pressed += Raise.Event();
            StringAssert.Contains(expected, stringWriter.ToString());
        }

        [Test]
        public void TestTimerExpired()
        {
            string expected = "Display cleared";
            PowerBt.Pressed += Raise.Event();
            TimeBt.Pressed += Raise.Event();
            cancelBt.Pressed += Raise.Event();
            ui.CookingIsDone();
            StringAssert.Contains(expected, stringWriter.ToString());
        }


    }
}
