using System;
using Xunit;
using IIG.PasswordHashingUtils;
using IIG.CoSFE.DatabaseUtils;

namespace XUnitTestProject3
{

    public class AuthDatabaseIntegrationTest
    {
        private const string Server = @"LAPTOP-RPCQUA9C\TESTSERVER";
        private const string Database = @"IIG.CoSWE.AuthDB";
        private const bool IsTrusted = false;
        private const string Login = @"sa";
        private const string Password = @"qwer";
        private const int ConnectionTimeout = 75;

        AuthDatabaseUtils auth = new AuthDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTimeout);

        [Fact]
        public void AddCredentialsTestParamsNull()
        {
            Assert.False(auth.AddCredentials(null, PasswordHasher.GetHash("1 test password")));
            Assert.Throws<ArgumentNullException>(() => auth.AddCredentials("1 test login", PasswordHasher.GetHash(null)));
            Assert.Throws<ArgumentNullException>(() => auth.AddCredentials(null, PasswordHasher.GetHash(null)));
        }

        [Fact]
        public void AddCredentialsTestParamsEmpty()
        {
            Assert.False(auth.AddCredentials("", PasswordHasher.GetHash("2 test password")));
            Assert.False(auth.AddCredentials("2 test login", PasswordHasher.GetHash("")));
            Assert.False(auth.AddCredentials("", PasswordHasher.GetHash("")));
        }

        [Fact]
        public void AddCredentialsTestNormal()
        {
            Assert.True(auth.AddCredentials("3 test login", PasswordHasher.GetHash("3 test password")));
            Assert.True(auth.AddCredentials("3 test login 2", PasswordHasher.GetHash("3 test password 2", "test salt", 32)));
            Assert.True(auth.AddCredentials("3 1234567890", PasswordHasher.GetHash("3 1234567890")));
        }

        [Fact]
        public void AddCredentialsTestSpecial()
        {
            Assert.True(auth.AddCredentials("4 test login", PasswordHasher.GetHash("4 ㅷ✅✈ぱ,/~")));
            Assert.True(auth.AddCredentials("4 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("4 test password")));
            Assert.True(auth.AddCredentials("4 ㅷ✅✈ぱ,/~ 2", PasswordHasher.GetHash("4 ㅷ✅✈ぱ,/~")));
        }

        [Fact]
        public void UpdateCredentialsTestNull()
        {
            auth.AddCredentials("5 test login", PasswordHasher.GetHash("5 test password"));

            Assert.Throws<ArgumentNullException>(() => auth.UpdateCredentials("5 test login", PasswordHasher.GetHash("5 test password"), null, PasswordHasher.GetHash(null)));
            Assert.Throws<ArgumentNullException>(() => auth.UpdateCredentials("5 test login", PasswordHasher.GetHash("5 test password"), "5 new login", PasswordHasher.GetHash(null)));
            Assert.False(auth.UpdateCredentials("5 test login", PasswordHasher.GetHash("5 test password"), null, PasswordHasher.GetHash("5 new password")));
        }

        [Fact]
        public void UpdateCredentialsTestEmpty()
        {
            auth.AddCredentials("6 test login", PasswordHasher.GetHash("6 test password"));

            Assert.False(auth.UpdateCredentials("6 test login", PasswordHasher.GetHash("6 test password"), "", PasswordHasher.GetHash("")));
            Assert.False(auth.UpdateCredentials("6 test login", PasswordHasher.GetHash("6 test password"), "", PasswordHasher.GetHash("6 new password")));
            Assert.False(auth.UpdateCredentials("6 test login", PasswordHasher.GetHash("6 test password"), "6 new login", PasswordHasher.GetHash("")));
        }

        [Fact]
        public void UpdateCredentialsNonExisting()
        {
            Assert.False(auth.UpdateCredentials("7 non existing login", PasswordHasher.GetHash("7 non existing password"), "7 new login", PasswordHasher.GetHash("7 new password")));
        }

        [Fact]
        public void UpdateCredentialsIncorrectPass()
        {
            auth.AddCredentials("8 test login", PasswordHasher.GetHash("8 test password"));

            Assert.False(auth.UpdateCredentials("8 test login", PasswordHasher.GetHash("8 wrong password"), "8 new login", PasswordHasher.GetHash("8 new password")));
        }

        [Fact]
        public void UpdateCredentialsSameParams()
        {
            auth.AddCredentials("9 test login", PasswordHasher.GetHash("9 test password"));

            Assert.False(auth.UpdateCredentials("9 test login", PasswordHasher.GetHash("9 test password"), "9 test login", PasswordHasher.GetHash("9 test password")));
        }

        [Fact]
        public void UpdateCredentialsTestNormal()
        {
            auth.AddCredentials("10 test login", PasswordHasher.GetHash("10 test password"));

            Assert.True(auth.UpdateCredentials("10 test login", PasswordHasher.GetHash("10 test password"), "10 new login", PasswordHasher.GetHash("10 new password")));
        }

        [Fact]
        public void UpdateCredentialsTestSpecial()
        {
            auth.AddCredentials("11 test login", PasswordHasher.GetHash("11 test password"));

            Assert.True(auth.UpdateCredentials("11 test login", PasswordHasher.GetHash("11 test password"), "11 new login", PasswordHasher.GetHash("11 ㅷ✅✈ぱ,/~")));
            Assert.True(auth.UpdateCredentials("11 new login", PasswordHasher.GetHash("11 ㅷ✅✈ぱ,/~"), "11 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("11 new password")));
            Assert.True(auth.UpdateCredentials("11 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("11 new password"), "11 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("11 ㅷ✅✈ぱ,/~")));
        }

        [Fact]
        public void CheckCredentialsTestNull()
        {
            auth.AddCredentials("12 test login", PasswordHasher.GetHash("12 test password"));

            Assert.Throws<ArgumentNullException>(() => auth.DeleteCredentials(null, PasswordHasher.GetHash(null)));
            Assert.Throws<ArgumentNullException>(() => auth.DeleteCredentials("12 test login", PasswordHasher.GetHash(null)));
            Assert.False(auth.CheckCredentials(null, PasswordHasher.GetHash("12 test password")));
        }

        [Fact]
        public void CheckCredentialsTestEmpty()
        {
            auth.AddCredentials("13 test login", PasswordHasher.GetHash("13 test password"));

            Assert.False(auth.CheckCredentials("", PasswordHasher.GetHash("")));
            Assert.False(auth.CheckCredentials("13 test login", PasswordHasher.GetHash("")));
            Assert.False(auth.CheckCredentials("", PasswordHasher.GetHash("13 test password")));
        }

        [Fact]
        public void CheckCredentialsNonExisting()
        {
            Assert.False(auth.CheckCredentials("14 non existing login", PasswordHasher.GetHash("14 non existing password")));
        }

        [Fact]
        public void CheckCredentialsIncorrectPass()
        {
            auth.AddCredentials("15 test login", PasswordHasher.GetHash("15 test password"));

            Assert.False(auth.CheckCredentials("15 test login", PasswordHasher.GetHash("15 wrong password")));
        }

        [Fact]
        public void CheckCredentialsTestNormal()
        {
            auth.AddCredentials("16 test login", PasswordHasher.GetHash("16 test password"));

            Assert.True(auth.CheckCredentials("16 test login", PasswordHasher.GetHash("16 test password")));
        }

        [Fact]
        public void CheckCredentialsTestSpecial()
        {
            auth.AddCredentials("17 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("17 ㅷ✅✈ぱ,/~"));

            Assert.True(auth.CheckCredentials("17 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("17 ㅷ✅✈ぱ,/~")));
        }

        [Fact]
        public void DeleteCredentialsTestNull()
        {
            auth.AddCredentials("18 test login", PasswordHasher.GetHash("18 test password"));

            Assert.Throws<ArgumentNullException>(() => auth.CheckCredentials(null, PasswordHasher.GetHash(null)));
            Assert.Throws<ArgumentNullException>(() => auth.CheckCredentials("18 test login", PasswordHasher.GetHash(null)));
            Assert.False(auth.CheckCredentials(null, PasswordHasher.GetHash("18 test password")));

            Assert.True(auth.CheckCredentials("18 test login", PasswordHasher.GetHash("18 test password")));
        }

        [Fact]
        public void DeleteCredentialsTestEmpty()
        {
            auth.AddCredentials("19 test login", PasswordHasher.GetHash("19 test password"));

            Assert.False(auth.CheckCredentials("", PasswordHasher.GetHash("")));
            Assert.False(auth.CheckCredentials("19 test login", PasswordHasher.GetHash("")));
            Assert.False(auth.CheckCredentials("", PasswordHasher.GetHash("19 test password")));

            Assert.True(auth.CheckCredentials("19 test login", PasswordHasher.GetHash("19 test password")));
        }

        [Fact]
        public void DeleteCredentialsNonExisting()
        {
            Assert.False(auth.CheckCredentials("20 non existing login", PasswordHasher.GetHash("20 non existing password")));
        }

        [Fact]
        public void DeleteCredentialsIncorrectPass()
        {
            auth.AddCredentials("21 test login", PasswordHasher.GetHash("21 test password"));

            Assert.False(auth.CheckCredentials("21 test login", PasswordHasher.GetHash("21 wrong password")));

            Assert.True(auth.CheckCredentials("21 test login", PasswordHasher.GetHash("21 test password")));
        }

        [Fact]
        public void DeleteCredentialsTestNormal()
        {
            PasswordHasher.Init("test salt", 16);
            auth.AddCredentials("22 test login", PasswordHasher.GetHash("22 test password"));

            Assert.True(auth.DeleteCredentials("22 test login", PasswordHasher.GetHash("22 test password")));

            Assert.False(auth.CheckCredentials("22 test login", PasswordHasher.GetHash("22 test password")));
        }

        [Fact]
        public void DeleteCredentialsTestSpecial()
        {
            auth.AddCredentials("23 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("23 ㅷ✅✈ぱ,/~"));

            Assert.True(auth.CheckCredentials("23 ㅷ✅✈ぱ,/~", PasswordHasher.GetHash("23 ㅷ✅✈ぱ,/~")));

            Assert.False(auth.CheckCredentials("23 test login", PasswordHasher.GetHash("23 test password")));
        }
    }
}
