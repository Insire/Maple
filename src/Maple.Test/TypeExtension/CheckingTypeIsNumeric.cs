using System;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class CheckingTypeIsNumeric
    {
        [TestMethod]
        public void RunIsNumericExtenionsTests()
        {
            typeof(bool).IsNumeric().ShouldBeFalse();
            typeof(string).IsNumeric().ShouldBeFalse();
            typeof(DateTime).IsNumeric().ShouldBeFalse();
            typeof(TimeSpan).IsNumeric().ShouldBeFalse();

            typeof(byte).IsNumeric().ShouldBeTrue();
            typeof(float).IsNumeric().ShouldBeTrue();
            typeof(decimal).IsNumeric().ShouldBeTrue();
            typeof(double).IsNumeric().ShouldBeTrue();
            typeof(short).IsNumeric().ShouldBeTrue();
            typeof(int).IsNumeric().ShouldBeTrue();
            typeof(long).IsNumeric().ShouldBeTrue();
            typeof(sbyte).IsNumeric().ShouldBeTrue();
            typeof(ushort).IsNumeric().ShouldBeTrue();
            typeof(uint).IsNumeric().ShouldBeTrue();
            typeof(ulong).IsNumeric().ShouldBeTrue();
        }
    }
}