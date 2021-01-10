using System;
using System.IO;
using Xunit;
using IIG.BinaryFlag;
using IIG.FileWorker;

namespace XUnitTestProject3
{
    public class FileWorkerIntegrationTest
    {
        private const string ReadAllFile = "C:\\Users\\vladi\\OneDrive\\Документи\\Study\\Testing\\XUnitTestProject3\\src\\TextFile1.txt";
        private const string ReadLinesFile = "C:\\Users\\vladi\\OneDrive\\Документи\\Study\\Testing\\XUnitTestProject3\\src\\TextFile2.txt";
        private const string WriteFile = "C:\\Users\\vladi\\OneDrive\\Документи\\Study\\Testing\\XUnitTestProject3\\src\\TextFile3.txt";

        public FileWorkerIntegrationTest()
        {
            File.WriteAllText(ReadAllFile, "False");
            File.WriteAllText(ReadLinesFile, "False\nTrue\nFalse\nFalse");
            File.WriteAllText(WriteFile, "");
        }

        [Fact]
        public void ReadAllTestEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3, false);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(55, false);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(2);
            flag3.ResetFlag(0);

            Assert.Equal(flag1.GetFlag().ToString(), BaseFileWorker.ReadAll(ReadAllFile));
            Assert.Equal(flag2.GetFlag().ToString(), BaseFileWorker.ReadAll(ReadAllFile));
            Assert.Equal(flag3.GetFlag().ToString(), BaseFileWorker.ReadAll(ReadAllFile));
        }

        [Fact]
        public void ReadAllTestNotEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(55);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(2, false);
            flag3.SetFlag(0);
            flag3.SetFlag(1);

            Assert.NotEqual(flag1.GetFlag().ToString(), BaseFileWorker.ReadAll(ReadAllFile));
            Assert.NotEqual(flag2.GetFlag().ToString(), BaseFileWorker.ReadAll(ReadAllFile));
            Assert.NotEqual(flag3.GetFlag().ToString(), BaseFileWorker.ReadAll(ReadAllFile));
        }

        [Fact]
        public void ReadLinesTestEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3, false);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(2);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(55, false);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(2);
            flag4.ResetFlag(0);

            string[] lines = BaseFileWorker.ReadLines(ReadLinesFile);

            Assert.Equal(4, lines.Length);
            Assert.Equal(flag1.GetFlag().ToString(), lines[0]);
            Assert.Equal(flag2.GetFlag().ToString(), lines[1]);
            Assert.Equal(flag3.GetFlag().ToString(), lines[2]);
            Assert.Equal(flag4.GetFlag().ToString(), lines[3]);
        }


        [Fact]
        public void ReadLinesTestNotEqual()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(2, false);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(55);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(2);
            flag4.SetFlag(0);
            flag4.SetFlag(1);

            string[] lines = BaseFileWorker.ReadLines(ReadLinesFile);

            Assert.Equal(4, lines.Length);
            Assert.NotEqual(flag1.GetFlag().ToString(), lines[0]);
            Assert.NotEqual(flag2.GetFlag().ToString(), lines[1]);
            Assert.NotEqual(flag3.GetFlag().ToString(), lines[2]);
            Assert.NotEqual(flag4.GetFlag().ToString(), lines[3]);
        }

        [Fact]
        public void WriteTestSingle()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3, false);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(55);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(2);
            flag3.ResetFlag(0);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(2, false);
            flag4.SetFlag(0);
            flag4.SetFlag(1);

            Assert.True(BaseFileWorker.Write(flag1.GetFlag().ToString(), WriteFile));
            Assert.Equal("False", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.Write(flag2.GetFlag().ToString(), WriteFile));
            Assert.Equal("True", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.Write(flag3.GetFlag().ToString(), WriteFile));
            Assert.Equal("False", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.Write(flag4.GetFlag().ToString(), WriteFile));
            Assert.Equal("True", BaseFileWorker.ReadAll(WriteFile));
        }


        [Fact]
        public void WriteTestEqualMultiline()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3, false);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(55);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(2);
            flag3.ResetFlag(0);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(2, false);
            flag4.SetFlag(0);
            flag4.SetFlag(1);

            Assert.True(BaseFileWorker.Write(flag1.GetFlag().ToString() + '\n' + flag2.GetFlag().ToString(), WriteFile));
            Assert.Equal("False\nTrue", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.Write(flag2.GetFlag().ToString() + '\n' + flag3.GetFlag().ToString() + '\n' 
                + flag4.GetFlag().ToString(), WriteFile));
            Assert.Equal("True\nFalse\nTrue", BaseFileWorker.ReadAll(WriteFile));
        }

        [Fact]
        public void TryWriteTestSingle()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3, false);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(55);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(2);
            flag3.ResetFlag(0);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(2, false);
            flag4.SetFlag(0);
            flag4.SetFlag(1);

            Assert.True(BaseFileWorker.TryWrite(flag1.GetFlag().ToString(), WriteFile));
            Assert.Equal("False", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.TryWrite(flag2.GetFlag().ToString(), WriteFile, 2));
            Assert.Equal("True", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.TryWrite(flag3.GetFlag().ToString(), WriteFile, 2));
            Assert.Equal("False", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.TryWrite(flag4.GetFlag().ToString(), WriteFile, 10));
            Assert.Equal("True", BaseFileWorker.ReadAll(WriteFile));
        }


        [Fact]
        public void TryWriteTestEqualMultiline()
        {
            MultipleBinaryFlag flag1 = new MultipleBinaryFlag(3, false);
            MultipleBinaryFlag flag2 = new MultipleBinaryFlag(55);
            MultipleBinaryFlag flag3 = new MultipleBinaryFlag(2);
            flag3.ResetFlag(0);
            MultipleBinaryFlag flag4 = new MultipleBinaryFlag(2, false);
            flag4.SetFlag(0);
            flag4.SetFlag(1);

            Assert.True(BaseFileWorker.TryWrite(flag1.GetFlag().ToString() + '\n' + flag2.GetFlag().ToString(), WriteFile));
            Assert.Equal("False\nTrue", BaseFileWorker.ReadAll(WriteFile));
            Assert.True(BaseFileWorker.TryWrite(flag2.GetFlag().ToString() + '\n' + flag3.GetFlag().ToString() + '\n' 
                + flag4.GetFlag().ToString(), WriteFile, 10));
            Assert.Equal("True\nFalse\nTrue", BaseFileWorker.ReadAll(WriteFile));
        }
    }
}
