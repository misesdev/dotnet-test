using api.Service;

namespace api.Tests.Service
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        public void GenerateSalt_ShouldReturnBase64StringOfExpectedLength()
        {
            var _passwordService = new PasswordService();

            var salt = _passwordService.GenerateSalt();
            var bytes = Convert.FromBase64String(salt);

            Assert.AreEqual(16, bytes.Length); // 16 bytes de salt
        }

        [TestMethod]
        public void HashPassword_ShouldReturnConsistentHashForSameInput()
        {
            var _passwordService = new PasswordService();
            
            var password = "StrongP@ssw0rd!";
            var salt = _passwordService.GenerateSalt();

            var hash1 = _passwordService.HashPassword(password, salt);
            var hash2 = _passwordService.HashPassword(password, salt);

            Assert.AreEqual(hash1, hash2); // Deve ser determinístico com mesmo salt
        }

        [TestMethod]
        public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            var _passwordService = new PasswordService();
            
            var password = "ValidP@ss1";
            var salt = _passwordService.GenerateSalt();
            var hash = _passwordService.HashPassword(password, salt);

            var result = _passwordService.VerifyPassword(password, hash, salt);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
        {
            var _passwordService = new PasswordService();
            
            var password = "ValidP@ss1";
            var salt = _passwordService.GenerateSalt();
            var hash = _passwordService.HashPassword(password, salt);

            var result = _passwordService.VerifyPassword("WrongPassword", hash, salt);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WeakPassword_ShouldReturnTrueForShortPassword()
        {
            var _passwordService = new PasswordService();
            
            Assert.IsTrue(_passwordService.WeakPassword("abc")); // Muito curta
        }

        [TestMethod]
        public void WeakPassword_ShouldReturnTrueIfNoDigit()
        {
            var _passwordService = new PasswordService();
            
            Assert.IsTrue(_passwordService.WeakPassword("NoDigits!"));
        }

        [TestMethod]
        public void WeakPassword_ShouldReturnTrueIfNoSymbol()
        {
            var _passwordService = new PasswordService();
            
            Assert.IsTrue(_passwordService.WeakPassword("NoSymbols123"));
        }

        [TestMethod]
        public void WeakPassword_ShouldReturnFalseForStrongPassword()
        {
            var _passwordService = new PasswordService();
            
            Assert.IsFalse(_passwordService.WeakPassword("Strong@123"));
        }
    }
}
