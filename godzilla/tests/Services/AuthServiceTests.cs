using Microsoft.EntityFrameworkCore;
using Moq;
using api.Data;
using api.Service;
using api.Models;
using api.Models.Common;
using api.Extentios;
using System.Linq;

namespace api.Tests.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        private AppDbContex GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContex>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContex(options);
        }

        private RecordUser GetSampleUser() => new RecordUser
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "StrongPassword123"
        };

        private SignUser GetSampleSignIn() => new SignUser
        {
            Email = "alice@example.com",
            Password = "StrongPassword123"
        };

        [TestMethod]
        public async Task SignUp_ShouldSucceed_WhenValidInput()
        {
            var context = GetInMemoryContext("SignUp_Success");

            var passwordMock = new Mock<PasswordService>();
            passwordMock.Setup(p => p.WeakPassword(It.IsAny<string>())).Returns(false);
            passwordMock.Setup(p => p.GenerateSalt()).Returns("salt");
            passwordMock.Setup(p => p.HashPassword(It.IsAny<string>(), It.IsAny<string>())).Returns("hashed");

            var tokenMock = new Mock<TokenService>();

            var service = new AuthService(context, passwordMock.Object, tokenMock.Object);
            var result = await service.SignUp(GetSampleUser());

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Usuario cadastrado com sucesso!", result.Message);
            Assert.AreEqual("alice@example.com", result.Data.Email);
        }

        [TestMethod]
        public async Task SignUp_ShouldFail_WhenEmailExists()
        {
            var context = GetInMemoryContext("SignUp_EmailExists");
            context.Users.Add(new User { Id = Guid.NewGuid(), Email = "alice@example.com", Name = "Test" });
            await context.SaveChangesAsync();

            var passwordMock = new Mock<PasswordService>();
            var tokenMock = new Mock<TokenService>();

            var service = new AuthService(context, passwordMock.Object, tokenMock.Object);
            var result = await service.SignUp(GetSampleUser());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail já existente! (Jamais seria exibido em cenário real)!", result.Message);
        }

        [TestMethod]
        public async Task SignUp_ShouldFail_WhenPasswordIsWeak()
        {
            var context = GetInMemoryContext("SignUp_WeakPassword");

            var passwordMock = new Mock<PasswordService>();
            passwordMock.Setup(p => p.WeakPassword(It.IsAny<string>())).Returns(true);

            var tokenMock = new Mock<TokenService>();

            var service = new AuthService(context, passwordMock.Object, tokenMock.Object);
            var result = await service.SignUp(GetSampleUser());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Senha muito fraca!", result.Message);
        }

        [TestMethod]
        public async Task Sigin_ShouldSucceed_WithCorrectCredentials()
        {
            var context = GetInMemoryContext("Sigin_Success");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "alice@example.com",
                Name = "Alice",
                Salt = "salt",
                PasswordHash = "hashed"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var passwordMock = new Mock<PasswordService>();
            passwordMock.Setup(p => p.VerifyPassword("StrongPassword123", "hashed", "salt")).Returns(true);

            var tokenMock = new Mock<TokenService>();
            tokenMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("mocked_token");

            var service = new AuthService(context, passwordMock.Object, tokenMock.Object);
            var result = await service.Sigin(GetSampleSignIn());

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data.Auth);
            Assert.AreEqual("mocked_token", result.Data.Token);
            Assert.AreEqual("alice@example.com", result.Data.User.Email);
        }

        [TestMethod]
        public async Task Sigin_ShouldFail_WhenUserNotFound()
        {
            var context = GetInMemoryContext("Sigin_UserNotFound");

            var passwordMock = new Mock<PasswordService>();
            var tokenMock = new Mock<TokenService>();

            var service = new AuthService(context, passwordMock.Object, tokenMock.Object);
            var result = await service.Sigin(GetSampleSignIn());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail ou senha inválidos!", result.Message);
        }

        [TestMethod]
        public async Task Sigin_ShouldFail_WhenPasswordIncorrect()
        {
            var context = GetInMemoryContext("Sigin_WrongPassword");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "alice@example.com",
                Name = "Alice",
                Salt = "salt",
                PasswordHash = "hashed"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var passwordMock = new Mock<PasswordService>();
            passwordMock.Setup(p => p.VerifyPassword("StrongPassword123", "hashed", "salt")).Returns(false);

            var tokenMock = new Mock<TokenService>();

            var service = new AuthService(context, passwordMock.Object, tokenMock.Object);
            var result = await service.Sigin(GetSampleSignIn());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail ou senha inválidos!", result.Message);
        }
    }
}
