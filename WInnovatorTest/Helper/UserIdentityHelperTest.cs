using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using WInnovator.Helper;
using WInnovatorTest.Helper.Fixture;
using Xunit;

namespace WInnovatorTest.Helper
{
    public class UserIdentityHelperTest : IClassFixture<UserIdentityHelperTestFixture>
    {
        private UserIdentityHelperTestFixture _fixture;

        public UserIdentityHelperTest(UserIdentityHelperTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateConfirmedUserIfNonExistentTestWithExistingUserWithSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CreateConfirmedUserIfNonExistent(_fixture._appUser.Email, "");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task CreateConfirmedUserIfNonExistentTestWithNonExistingUserWithSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CreateConfirmedUserIfNonExistent(_fixture._normalUser.Email, "");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task CreateConfirmedUserIfNonExistentTestWithNonExistingUserWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CreateConfirmedUserIfNonExistent("blabla@blabla.bla", "");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task AddRoleToUserTestWithUserWithoutRoleSuccess()
        {
            // Arrange
            var result = await _fixture._controller.AddRoleToUser(_fixture._normalUser.Email, _fixture._appUserRole);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task AddRoleToUserTestWithNonExistingRoleWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.AddRoleToUser(_fixture._normalUser.Email, "NonExistingRole");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task AddRoleToUserTestWithNonExistingUserWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.AddRoleToUser("NonExistingUser", _fixture._appUserRole);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }
        
        [Fact]
        public async Task CreateRoleIfNonExistentTestWithExistingRoleSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CreateRoleIfNonExistent(_fixture._appUserRole);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task CreateRoleIfNonExistentTestWithNonExistingRoleSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CreateRoleIfNonExistent("newRole");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task SearchUserTestWithValidAccountSuccess()
        {
            // Arrange
            var result = await _fixture._controller.SearchUser(_fixture._appUser.Email);
            
            // Assert
            var actualResult = Assert.IsType<IdentityUser>(result);
            Assert.Equal(_fixture._appUser, actualResult);
        }

        [Fact]
        public async Task SearchUserTestWithInvalidAccountSuccess()
        {
            // Arrange
            var result = await _fixture._controller.SearchUser("NonExistingAccount");
            
            // Assert
            Assert.True(result==null);
        }

        [Fact]
        public async Task CredentialsAreValidTestWithValidCredentialsSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CredentialsAreValid(_fixture._normalUser.Email, _fixture._validPassword);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task CredentialsAreValidTestWithInvalidCredentialsSuccess()
        {
            // Arrange
            var result = await _fixture._controller.CredentialsAreValid(_fixture._normalUser.Email, "invalidPassword");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task GetAllRolesForUserTestCheckOutput()
        {
            // Arrange
            var result = await _fixture._controller.GetAllRolesForUser(_fixture._appUser);
            
            // Assert
            var actualResult = Assert.IsType<List<string>>(result);
            Assert.Equal(_fixture._appUserRoles, actualResult);
        }

        [Fact]
        public async Task RemoveAppUserTestWithValidAppUserSuccess()
        {
            // Arrange
            var result = await _fixture._controller.RemoveAppUser(_fixture._appUser.Email);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task RemoveAppUserTestWithNormalUserWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.RemoveAppUser(_fixture._normalUser.Email);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task RemoveAppUserTestWithInvalidUserWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.RemoveAppUser("blabla@bla.bla");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public void ConstructAppUsername()
        {
            int LengthOfGuid = 32;
            int LengthOfDefault = DefaultUsersAndRoles.defaultMailPartOfAppUserAccounts.Length;
            int totalLength = LengthOfGuid + LengthOfDefault;

            string output = _fixture._controller.ConstructAppUsername();
            Assert.Equal(totalLength, output.Length);
            
            IdentityUser fromOutput = new IdentityUser() { Email = output};
            Assert.True(fromOutput.IsAppUserAccount());
        }

        [Fact]
        public async Task RemoveAllRolesFromUserTestWithValidAppUserSuccess()
        {
            // Arrange
            var result = await _fixture._controller.RemoveAllRolesFromUser(_fixture._appUser.Email);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task RemoveAllRolesFromUserTestWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.RemoveAllRolesFromUser(_fixture._normalUser.Email);
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task RemoveAllRolesFromUserTestWithInvalidUserWithoutSuccess()
        {
            // Arrange
            var result = await _fixture._controller.RemoveAllRolesFromUser("blabla@bla.bla");
            
            // Assert
            var actualResult = Assert.IsType<bool>(result);
            Assert.False(actualResult);
        }

        [Fact]
        public void GenerateNotBeforeClaimTestWithAppUserAccount()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GenerateNotBeforeClaim", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUser });
            
            // Assert
            var actualResult = Assert.IsType<Task<Claim>>(result);
            Claim claimResult = (Claim) Assert.IsType<Claim>(actualResult.Result);
            Assert.True(JwtRegisteredClaimNames.Nbf.Equals(claimResult.Type));
            Assert.Equal(_fixture._shopWithAppUser.Date.Date, DateTimeOffset.FromUnixTimeSeconds(long.Parse(claimResult.Value)).Date);
        }

        [Fact]
        public void GenerateNotBeforeClaimTestWithNormalUserAccount()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GenerateNotBeforeClaim", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._normalUser });
            
            // Assert
            var actualResult = Assert.IsType<Task<Claim>>(result);
            Claim claimResult = (Claim) Assert.IsType<Claim>(actualResult.Result);
            Assert.True(JwtRegisteredClaimNames.Nbf.Equals(claimResult.Type));
            Assert.Equal(DateTime.Now.Date, DateTimeOffset.FromUnixTimeSeconds(long.Parse(claimResult.Value)).Date);
        }

        [Fact]
        public void GenerateExpiresAfterClaimTestWithAppUserAccount()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GenerateExpiresAfterClaim", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUser });
            
            // Assert
            var actualResult = Assert.IsType<Task<Claim>>(result);
            Claim claimResult = (Claim) Assert.IsType<Claim>(actualResult.Result);
            Assert.True(JwtRegisteredClaimNames.Exp.Equals(claimResult.Type));
            Assert.Equal(_fixture._shopWithAppUser.Date.Date, DateTimeOffset.FromUnixTimeSeconds(long.Parse(claimResult.Value)).Date);
        }

        [Fact]
        public void GenerateExpiresAfterClaimTestWithNormalUserAccount()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GenerateExpiresAfterClaim", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._normalUser });
            
            // Assert
            var actualResult = Assert.IsType<Task<Claim>>(result);
            Claim claimResult = (Claim) Assert.IsType<Claim>(actualResult.Result);
            Assert.True(JwtRegisteredClaimNames.Exp.Equals(claimResult.Type));
            Assert.Equal(DateTime.Now.AddDays(1).Date, DateTimeOffset.FromUnixTimeSeconds(long.Parse(claimResult.Value)).Date);
        }

        [Fact]
        public void GetDesignShopDateTest()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GetDesignShopDate", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUser });
            
            // Assert
            var actualResult = Assert.IsType<Task<DateTime>>(result);
            DateTime dateResult = (DateTime) Assert.IsType<DateTime>(actualResult.Result);
            Assert.Equal(_fixture._shopWithAppUser.Date, dateResult);
        }

        [Fact]
        public void UserHasRoleTestWithUserWithRole()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("UserHasRole", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUser, _fixture._appUserRole });
            
            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.True(actualResult.Result);
        }

        [Fact]
        public void UserHasRoleTestWithUserWithoutRole()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("UserHasRole", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUser, "unavailable" });
            
            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.False(actualResult.Result);
        }

        [Fact]
        public void CreateRoleTestWithSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("CreateRole", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { "newRole" });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.True(actualResult.Result);
        }

        [Fact]
        public void CreateRoleTestWithoutSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("CreateRole", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] {  _fixture._appUserRole });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.False(actualResult.Result);
        }

        [Fact]
        public void GiveUserTheNewRoleTestWithSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GiveUserTheNewRole", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._normalUser, _fixture._appUserRole });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.True(actualResult.Result);
        }

        [Fact]
        public void GiveUserTheNewRoleTestWithoutSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("GiveUserTheNewRole", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._normalUser, "NonExistingRole" });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.False(actualResult.Result);
        }
        
        [Fact]
        public void GenerateSecurePasswordTest()
        {
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var pw = t.GetMethod("GenerateSecurePassword", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { });
            Assert.IsType<string>(pw);
            string password = (string) pw;
            Assert.Matches(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)", password);
            Assert.True(password.Length==64);
        }
        
        [Fact]
        public void CreateConfirmedUserTestWithSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("CreateConfirmedUser", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._normalUser.Email, "password" });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.True(actualResult.Result);
        }

        [Fact]
        public void CreateConfirmedUserTestWithoutSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("CreateConfirmedUser", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUser.Email, "password" });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.False(actualResult.Result);
        }
        
        [Fact]
        public void RoleExistsTestWithSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("RoleExists", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { _fixture._appUserRole });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.True(actualResult.Result);
        }

        [Fact]
        public void RoleExistsTestWithoutSucces()
        {
            // Arrange
            object o = _fixture._controller;
            var t = typeof(UserIdentityHelper);
            var result = t.GetMethod("RoleExists", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { "NonExistingRole" });

            // Assert
            var actualResult = Assert.IsType<Task<bool>>(result);
            Assert.False(actualResult.Result);
        }
    }
}