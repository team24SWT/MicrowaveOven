using Microwave.Classes.Boundary;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    class LightToOutputTest
    {
        private Light light_;
        private Output output_;
        private StringWriter SW_;

        [SetUp]
        public void Setup()
        {
            output_ = new Output();
            light_ = new Light(output_);
            SW_ = new StringWriter();
            Console.SetOut(SW_);
        }

        [Test]
        public void TurnOn()
        {
            light_.TurnOn();
            string str = "Light is turned on";
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TurnOnThenTurnOff()
        {
            light_.TurnOn();
            light_.TurnOff();
            string str = "Light is turned off";
            StringAssert.Contains(str, SW_.ToString());
        }

        [Test]
        public void TurnOffOnly()
        {
            light_.TurnOff();
            string str = "Light is turned off";
            StringAssert.DoesNotContain(str, SW_.ToString());
        }
    }
}
