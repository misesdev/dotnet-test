using Moq;
using System.Linq.Expressions;
using api.Models;
using api.Service;
using api.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Service;

[TestClass]
public class AuthServiceTests
{
    private readonly AppDbContex _dbContext;
    private readonly Mock<PasswordService> _passwordMock;
    private readonly Mock<TokenService> _tokenMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _passwordMock = new Mock<PasswordService>();
        _tokenMock = new Mock<TokenService>();

        _authService = new AuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
    }

    [TestMethod]
    public async Task SignUp_ShouldFail_WhenEmailExists()
    {
        // Arrange
        var model = new RecordUser { Email = "test@example.com", Password = "abc123", Name = "Test" };

        var service = new TestableAuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
        service.SetExistsResult(true);

        // Act
        var result = await service.SignUp(model);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("E-mail já existente");
    }

    [TestMethod]
    public async Task SignUp_ShouldFail_WhenPasswordIsWeak()
    {
        // Arrange
        var model = new RecordUser { Email = "test@example.com", Password = "123", Name = "Weak" };

        var service = new TestableAuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
        service.SetExistsResult(false);
        _passwordMock.Setup(p => p.WeakPassword(model.Password)).Returns(true);

        // Act
        var result = await service.SignUp(model);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("fraca");
    }

    [TestMethod]
    public async Task SignUp_ShouldSucceed_WhenValidData()
    {
        // Arrange
        var model = new RecordUser { Email = "test@example.com", Password = "abc123", Name = "Valid" };

        var service = new TestableAuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
        service.SetExistsResult(false);

        _passwordMock.Setup(p => p.WeakPassword(model.Password)).Returns(false);
        _passwordMock.Setup(p => p.GenerateSalt()).Returns("SALT");
        _passwordMock.Setup(p => p.HashPassword(model.Password, "SALT")).Returns("HASH");

        // Act
        var result = await service.SignUp(model);

        // Assert
        result.Success.Should().BeTrue();
        result.Data?.Email.Should().Be(model.Email);
    }

    [TestMethod]
    public async Task Sigin_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        var model = new SignUser { Email = "notfound@example.com", Password = "pass" };

        var service = new TestableAuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
        service.SetFindUser(null);

        // Act
        var result = await service.Sigin(model);

        // Assert
        result.Success.Should().BeFalse();
    }

    [TestMethod]
    public async Task Sigin_ShouldFail_WhenPasswordIsInvalid()
    {
        // Arrange
        var model = new SignUser { Email = "user@example.com", Password = "wrong" };
        var user = new User { Email = model.Email, PasswordHash = "HASH", Salt = "SALT" };

        var service = new TestableAuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
        service.SetFindUser(user);

        _passwordMock.Setup(p => p.VerifyPassword(model.Password, user.PasswordHash, user.Salt)).Returns(false);

        // Act
        var result = await service.Sigin(model);

        // Assert
        result.Success.Should().BeFalse();
    }

    [TestMethod]
    public async Task Sigin_ShouldSucceed_WhenCredentialsAreCorrect()
    {
        // Arrange
        var model = new SignUser { Email = "user@example.com", Password = "correct" };
        var user = new User { Email = model.Email, PasswordHash = "HASH", Salt = "SALT", Name = "User", Id = Guid.NewGuid() };

        var service = new TestableAuthService(_dbContext, _passwordMock.Object, _tokenMock.Object);
        service.SetFindUser(user);

        _passwordMock.Setup(p => p.VerifyPassword(model.Password, user.PasswordHash, user.Salt)).Returns(true);
        _tokenMock.Setup(t => t.GenerateToken(user)).Returns("TOKEN123");

        // Act
        var result = await service.Sigin(model);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Token.Should().Be("TOKEN123");
    }

    // Classe auxiliar para mockar métodos protegidos de BaseService
    private class TestableAuthService : AuthService
    {
        private bool _existsResult = false;
        private User _foundUser;

        public TestableAuthService(AppDbContex context, PasswordService password, TokenService token)
            : base(context, password, token) { }

        public void SetExistsResult(bool exists) => _existsResult = exists;

        public void SetFindUser(User user) => _foundUser = user;

        protected override Task<bool> Exists(Expression<Func<User, bool>> predicate)
            => Task.FromResult(_existsResult);

        protected override Task<User?> FindAsync(Expression<Func<User, bool>> predicate)
            => Task.FromResult(_foundUser);
    }
}

