using System;
using Xunit;
using IIG.BinaryFlag;

namespace XUnitTestProject3
{
    public class BinaryFlagTest
    {

        [Fact]
        public void ConstructorTestLengthOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MultipleBinaryFlag(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MultipleBinaryFlag(17179868705));
        }

        [Fact]
        public void ConstructorTestLengthRange()
        {
            Assert.NotNull(new MultipleBinaryFlag(2));
            Assert.NotNull(new MultipleBinaryFlag(32));
            Assert.NotNull(new MultipleBinaryFlag(33));
            Assert.NotNull(new MultipleBinaryFlag(64));
            Assert.NotNull(new MultipleBinaryFlag(65));
            Assert.NotNull(new MultipleBinaryFlag(17179868704));
        }

        [Fact]
        public void GetFlagTestNotEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(32);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(32, false);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(16, false);

            Assert.NotEqual(flag1, flag2);
            Assert.NotEqual(flag2, flag3);
            Assert.NotEqual(flag1, flag3);
            Assert.NotEqual(flag1.GetFlag(), flag2.GetFlag());
            Assert.NotEqual(flag1.GetFlag(), flag3.GetFlag());
        }

        [Fact]
        public void GetFlagTestEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(32);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(32);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(16, false);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(16, false);

            Assert.Equal(flag1.GetFlag(), flag2.GetFlag());
            Assert.Equal(flag3.GetFlag(), flag4.GetFlag());
        }

        [Fact]
        public void SetFlagTestOutOfRange()
        {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(3, false);

            flag.SetFlag(0);
            Assert.Throws<ArgumentOutOfRangeException>(() => flag.SetFlag(3));
        }

        [Fact]
        public void GetSetFlagTest()
        {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(3, false);

            Assert.False(flag.GetFlag());
            flag.SetFlag(0);
            Assert.False(flag.GetFlag());
            flag.SetFlag(1);
            flag.SetFlag(2);
            Assert.True(flag.GetFlag());
        }

        [Fact]
        public void ResetFlagTest()
        {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(3, false);

            flag.SetFlag(0);
            flag.SetFlag(1);
            flag.SetFlag(2);
            Assert.True(flag.GetFlag());
            flag.ResetFlag(0);
            Assert.False(flag.GetFlag());
            flag.ResetFlag(1);
            flag.ResetFlag(2);
            Assert.False(flag.GetFlag());
        }

        [Fact]
        public void DisposeTest()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(32);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(32, false);
            flag1.Dispose();
            flag2.Dispose();

            Assert.NotNull(flag1);
            Assert.NotNull(flag2);
            Assert.True(flag1.GetFlag());
            Assert.False(flag2.GetFlag());
        }

        [Fact]
        public void GetTypeTestEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(32);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(32, false);

            Assert.True(flag1.GetType().Equals(flag2.GetType()));
            Assert.Equal("IIG.BinaryFlag.MultipleBinaryFlag", flag1.GetType().ToString());
            Assert.Equal("IIG.BinaryFlag.MultipleBinaryFlag", flag2.GetType().ToString());
        }

        [Fact]
        public void ToStringTest()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(3, false);

            Assert.Equal("TTT", flag1.ToString());
            Assert.Equal("FFF", flag2.ToString());

            flag1.ResetFlag(1);
            Assert.Equal("TFT", flag1.ToString());

            flag1.SetFlag(1);
            Assert.Equal("TTT", flag1.ToString());

            flag1.SetFlag(0);
            flag1.SetFlag(1);
            flag1.SetFlag(2);
            Assert.Equal("TTT", flag1.ToString());
        }
    }
}