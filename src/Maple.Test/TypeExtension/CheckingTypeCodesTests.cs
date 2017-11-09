using System;
using System.Threading.Tasks;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class CheckingTypeCodesTests
    {
        [TestMethod]
        public void Run()
        {
            typeof(bool).GetTypeCode().ShouldBe(TypeCode.Boolean);
            typeof(char).GetTypeCode().ShouldBe(TypeCode.Char);
            typeof(sbyte).GetTypeCode().ShouldBe(TypeCode.SByte);
            typeof(byte).GetTypeCode().ShouldBe(TypeCode.Byte);
            typeof(short).GetTypeCode().ShouldBe(TypeCode.Int16);
            typeof(ushort).GetTypeCode().ShouldBe(TypeCode.UInt16);
            typeof(int).GetTypeCode().ShouldBe(TypeCode.Int32);
            typeof(uint).GetTypeCode().ShouldBe(TypeCode.UInt32);
            typeof(long).GetTypeCode().ShouldBe(TypeCode.Int64);
            typeof(ulong).GetTypeCode().ShouldBe(TypeCode.UInt64);
            typeof(float).GetTypeCode().ShouldBe(TypeCode.Single);
            typeof(double).GetTypeCode().ShouldBe(TypeCode.Double);
            typeof(decimal).GetTypeCode().ShouldBe(TypeCode.Decimal);
            typeof(DateTime).GetTypeCode().ShouldBe(TypeCode.DateTime);
            typeof(string).GetTypeCode().ShouldBe(TypeCode.String);

            typeof(TimeSpan).GetTypeCode().ShouldBe(TypeCode.Object);
            typeof(Task).GetTypeCode().ShouldBe(TypeCode.Object);
        }
    }
}