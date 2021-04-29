using System;
using NUnit.Framework;
using Microwave.Classes.Boundary;
using System.IO;

namespace Microwave.Test.Integration
{
    class PowerTubeToOutputTest
    {
        private Output output_;
        private PowerTube PT_;
        private StringWriter SW_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            PT_ = new PowerTube(output_);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
        }

        [Test]
        public void TurnOn()
        {
            PT_.TurnOn(50);
            StringAssert.Contains("PowerTube works with 50", SW_.ToString());
        }
        [Test]
        public void TurnOnThenTurnOff()
        {
            PT_.TurnOn(50);
            PT_.TurnOff();
            StringAssert.Contains("PowerTube turned off", SW_.ToString());
        }
        [Test]
        public void TurnOff()
        {
            PT_.TurnOff();
            StringAssert.DoesNotContain("PowerTube turned off", SW_.ToString());
        }
    }
}
