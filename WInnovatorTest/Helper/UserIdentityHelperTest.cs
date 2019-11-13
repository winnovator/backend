using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WInnovator.Helper;
using WInnovatorTest.Helper.Fixture;
using Xunit;

namespace WInnovatorTest.Helper
{
    [TestCaseOrderer("WInnovatorTest.XUnit.AlphabeticalOrderer", "WInnovatorTest")]
    public class UserIdentityHelperTest : IClassFixture<UserIdentityHelperTestFixture>
    {
        private UserIdentityHelperTestFixture _fixture;

        public UserIdentityHelperTest(UserIdentityHelperTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestOutputGenerateSecurePassword()
        {
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var pw = t.GetMethod("GenerateSecurePassword", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { });
            Assert.IsType<string>(pw);
            string password = (string) pw;
            Assert.Matches(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)", password);
            Assert.True(password.Length==64);
        }
    }
}