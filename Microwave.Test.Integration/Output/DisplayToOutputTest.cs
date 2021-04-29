using Microwave.Classes.Boundary;
using NUnit.Framework;
using System;
using System.IO;


namespace Microwave.Test.Integration
{
    class DisplayToOutputTest
    {
        private Display display_;
        private Output output_;
        private StringWriter SW_;

        [SetUp]

        public void Setup()
        {
            output_ = new Output();
            display_ = new Display(output_);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
        }

        [Test]
        public void ShowsCorrectShowTime()
        {
            int min = 6;
            int sec = 30;
            var str = "Display shows: 06:30";
            display_.ShowTime(min, sec);
            StringAssert.Contains(str, SW_.ToString());
        }

        [TestCase(50)]
        [TestCase(250)]
        [TestCase(450)]
        [TestCase(700)]

        public void ShowsCorrectShowPower(int power)
        {
            var str = $"Display shows: {power} W";
            display_.ShowPower(power);
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void ShowsDisplayCleared()
        {
            var str = "Display cleared";
            display_.Clear();
            StringAssert.Contains(str, SW_.ToString());
        }
    }
}
