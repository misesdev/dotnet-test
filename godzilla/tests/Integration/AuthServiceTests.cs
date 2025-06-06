using api.Models;
using api.Service;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Config;
using Microsoft.Extensions.Options;
namespace api.Tests.Integration;

[TestClass]
[TestCategory("Integration")]
public class AuthServiceTests 
{
    private static AuthService _authService = null!;
    private static AppDbContex _dbContext = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context) 
    {
        var options = new DbContextOptionsBuilder<AppDbContex>()
            .UseInMemoryDatabase("AuthServiceTestsDb") // Nome fixo para manter o estado
            .Options;

        _dbContext = new AppDbContex(options);
        //_dbContext.Database.EnsureCreated();

        var jwtOptions = Options.Create(new JwtSettings
        {
            Key = "4fe9dd19ebae4d8ca87d802bdc4239aa4e91743bdce84b0e9243b29b7fa616b6",
            Issuer = "test",
            Audience = "test",
            ExpireMinutes = 60
        });

        var passwordService = new PasswordService();
        var tokenService = new TokenService(jwtOptions);

        _authService = new AuthService(_dbContext, passwordService, tokenService);
    }

    [TestMethod]
    [Priority(1)]
    public async Task Test01_ShouldSignUpNewUser()
    {
        var model = new RecordUser
        {
            Name = "Alice Test",
            Email = "alice@test.com",
            Password = "StrongPassword123!"
        };

        var response = await _authService.SignUp(model);

        Assert.IsTrue(response.Success);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual("alice@test.com", response.Data.Email);
    }

    [TestMethod]
    [Priority(2)]
    public async Task Test02_ShouldFailSignUpForDuplicateEmail()
    {
        var model = new RecordUser
        {
            Name = "Alice Duplicate",
            Email = "alice@test.com", // mesmo email do anterior
            Password = "StrongPassword123!"
        };

        var response = await _authService.SignUp(model);
        Console.WriteLine(response.Message);

        Assert.IsFalse(response.Success);
        Assert.AreEqual("E-mail já existente! (Jamais seria exibido em cenário real)!", response.Message);
    }

    [TestMethod]
    [Priority(3)]
    public async Task Test03_ShouldFailSignUpForWeakPassword()
    {
        var model = new RecordUser
        {
            Name = "Weak Pass",
            Email = "weakpass@test.com",
            Password = "123" // senha fraca
        };

        var response = await _authService.SignUp(model);

        Assert.IsFalse(response.Success);
        Assert.AreEqual("Senha muito fraca!", response.Message);
    }

    [TestMethod]
    [Priority(4)]
    public async Task Test04_ShouldSignInWithCorrectCredentials()
    {
        var model = new SignUser
        {
            Email = "alice@test.com",
            Password = "StrongPassword123!"
        };

        var response = await _authService.Sigin(model);

        Assert.IsTrue(response.Success);
        Assert.IsNotNull(response.Data);
        Assert.IsTrue(response.Data.Auth);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Token));
    }

    [TestMethod]
    [Priority(5)]
    public async Task Test05_ShouldFailSignInWithWrongPassword()
    {
        var model = new SignUser
        {
            Email = "alice@test.com",
            Password = "WrongPassword"
        };

        var response = await _authService.Sigin(model);

        Assert.IsFalse(response.Success);
        Assert.AreEqual("E-mail ou senha inválidos!", response.Message);
    }

    [TestMethod]
    [Priority(6)]
    public async Task Test06_ShouldFailSignInWithUnknownEmail()
    {
        var model = new SignUser
        {
            Email = "notfound@test.com",
            Password = "AnyPassword"
        };

        var response = await _authService.Sigin(model);

        Assert.IsFalse(response.Success);
        Assert.AreEqual("E-mail ou senha inválidos!", response.Message);
    }
}
