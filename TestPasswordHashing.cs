using System;
using Xunit;
using IIG.PasswordHashingUtils;

namespace XUnitTestProject3
{
    public class PasswordHashingTest
    {
        private const int MaxStringLength = 1024;
        private const int MaxUint = 256;
        private Random random = new Random();
        private string GenerateRandomString()
        {
            var result = new char[random.Next(MaxStringLength)];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (char)random.Next(256);
            }

            return new string(result);
        }
        private uint GenerateRandomUint()
        {
            return (uint)random.Next(MaxUint);
        }


        [Fact]
        public void GetHashTestParamsNull()
        {
            Assert.Throws<ArgumentNullException>(() => PasswordHasher.GetHash(null, null, null));
            Assert.Throws<ArgumentNullException>(() => PasswordHasher.GetHash(null, null, 16));
            Assert.Throws<ArgumentNullException>(() => PasswordHasher.GetHash(null, "test salt", 0));
            PasswordHasher.GetHash("test password", null, 0);
        }

        [Fact]
        public void GetHashTestParamsEmpty()
        {
            PasswordHasher.GetHash("", "", 0);
            PasswordHasher.GetHash("", "", 16);
            PasswordHasher.GetHash("", "test salt", 0);
            PasswordHasher.GetHash("test password", "", 0);
        }

        [Fact]
        public void GetHashTestParamsSpecial()
        {
            PasswordHasher.GetHash("test password", "test salt", 7057);
            PasswordHasher.GetHash("ㅷ✅✈ぱ,/~", "test salt", 16);
            PasswordHasher.GetHash("test password", "み✷☃ㅊ+]@", 16);
        }

        [Fact]
        public void GetHashTestPassPartial()
        {
            string password = "test password";
            string hash = PasswordHasher.GetHash(password);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.NotEqual(hash, password);
        }

        [Fact]
        public void GetHashTestSaltPartial()
        {
            string password = "test password";
            string hash = PasswordHasher.GetHash(password, "test salt");

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.NotEqual(hash, password);
        }

        [Fact]
        public void GetHashTestFull()
        {
            string password = "test password";
            string hash = PasswordHasher.GetHash(password, "test salt", 16);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.NotEqual(hash, password);
        }

        [Fact]
        public void GetHashTestSpecial()
        {
            string password = "ㅷ✅✈ぱ,/~";

            string hash = PasswordHasher.GetHash(password, "み✷☃ㅊ+]@", 7057);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.NotEqual(hash, password);
        }

        [Fact]
        public void GetHashTestRandomPass()
        {
            string password = GenerateRandomString();
            string hash = PasswordHasher.GetHash(password);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.NotEqual(hash, password);
        }

        [Fact]
        public void GetHashTestRandomFullEqual()
        {
            string password = GenerateRandomString();
            string salt = GenerateRandomString();
            uint adler = GenerateRandomUint();
            string hash1 = PasswordHasher.GetHash(password, salt, adler);
            string hash2 = PasswordHasher.GetHash(password, salt, adler);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashTestRandomPassNotEqual()
        {
            string password1 = GenerateRandomString();
            string password2 = GenerateRandomString();
            string salt = GenerateRandomString();
            uint adler = GenerateRandomUint();
            string hash1 = PasswordHasher.GetHash(password1, salt, adler);
            string hash2 = PasswordHasher.GetHash(password2, salt, adler);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashTestRandomSaltNotEqual()
        {
            string password = GenerateRandomString();
            string salt1 = GenerateRandomString();
            string salt2 = GenerateRandomString();
            uint adler = GenerateRandomUint();
            string hash1 = PasswordHasher.GetHash(password, salt1, adler);
            string hash2 = PasswordHasher.GetHash(password, salt2, adler);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashTestRandomAdlerNotEqual()
        {
            string password = GenerateRandomString();
            string salt = GenerateRandomString();
            uint adler1 = GenerateRandomUint();
            uint adler2 = GenerateRandomUint();
            string hash1 = PasswordHasher.GetHash(password, salt, adler1);
            string hash2 = PasswordHasher.GetHash(password, salt, adler2);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void InitTestEqual()
        {
            string password = "test password";
            string salt = "test salt";
            uint adler = 16;

            PasswordHasher.Init(salt, adler);
            string hash1 = PasswordHasher.GetHash(password);
            string hash2 = PasswordHasher.GetHash(password, salt, adler);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void InitTestNotEqual()
        {
            string password = "test password";
            string salt1 = "test salt 1";
            string salt2 = "test salt 2";
            uint adler1 = 16;
            uint adler2 = 32;

            PasswordHasher.Init(salt1, adler1);
            string hash1 = PasswordHasher.GetHash(password);
            string hash2 = PasswordHasher.GetHash(password, salt2, adler2);

            Assert.NotEqual(hash1, hash2);
        }
    }
}